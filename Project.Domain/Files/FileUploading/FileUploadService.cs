using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;
using Project.Domain.Exceptions;
using Project.Generic;

namespace Project.Domain.Files.FileUploading;
public class FileUploadService : IFileUploadService
{
    private readonly IConfiguration _configuration;
    private readonly ShareClient _shareClient;

    public FileUploadService(IConfiguration configuration)
    {
        _configuration = configuration;
        _shareClient = new ShareClient(_configuration.GetConnectionString("Azure:StorageAccount")!, _configuration.GetConnectionString("Azure:ShareName")!);
    }

    public async Task<IFileModel> UploadFile(IEnumerable<Guid> directories, FileUploadModel file)
    {
        if (file.Content is null)
        {
            throw new ValidationException("File content is required");
        }
        var directoryClient = await GetOrCreateDirectory(directories);

        file.CreateNewFileName();
        var fileClient = directoryClient.GetFileClient(file.FullNewFileName());

        using var fileStream = file.GetContentAsStream()!;

        await fileClient.CreateAsync(fileStream.Length);
        var uploadResult = await fileClient.UploadAsync(fileStream);

        file.ContentHash = uploadResult.Value.ContentHash.GetBytesAsHexString();

        return file;
    }

    public async Task<IFileModel> DownloadFile(IEnumerable<Guid> directories, FileDownloadRequest request)
    {
        var directoryClient = await GetDirectory(directories);
        var fileClient = directoryClient.GetFileClient(request.FullNewFileName());
        if (!await fileClient.ExistsAsync())
        {
            throw new DomainFileNotFoundException($"File '{request.FullNewFileName()}' not found");
        }
        var downloadResult = await fileClient.DownloadAsync();

        var file = new FileDownloadModel
        {
            OriginalFileName = request.OriginalFileName,
            NewFileName = request.NewFileName,
            Extension = request.Extension,
        };
        file.SetContentFromStream(downloadResult.Value.Content);

        return file;
    }

    private async Task<ShareDirectoryClient> GetOrCreateDirectory(IEnumerable<Guid> directories)
    {
        ShareDirectoryClient directoryClient = _shareClient.GetRootDirectoryClient();
        foreach (var dir in directories)
        {
            directoryClient = await GetOrCreateDirectory(directoryClient, dir);
        }
        return directoryClient;
    }
    private static async Task<ShareDirectoryClient> GetOrCreateDirectory(ShareDirectoryClient shareDirectoryClient, Guid directory)
    {
        ShareDirectoryClient subDirectoryClient = shareDirectoryClient.GetSubdirectoryClient(directory.ToString());
        if (!await subDirectoryClient.ExistsAsync())
        {
            await subDirectoryClient.CreateAsync();
        }
        return subDirectoryClient;
    }
    private async Task<ShareDirectoryClient> GetDirectory(IEnumerable<Guid> directories)
    {
        ShareDirectoryClient directoryClient = _shareClient.GetRootDirectoryClient();
        foreach (var dir in directories)
        {
            directoryClient = await GetDirectory(directoryClient, dir);
        }
        return directoryClient;
    }
    private static async Task<ShareDirectoryClient> GetDirectory(ShareDirectoryClient shareDirectoryClient, Guid directory)
    {
        ShareDirectoryClient subDirectoryClient = shareDirectoryClient.GetSubdirectoryClient(directory.ToString());
        if (!await subDirectoryClient.ExistsAsync())
        {
            throw new InvalidOperationException($"Directory '{directory}' not found");
        }
        return subDirectoryClient;
    }
}