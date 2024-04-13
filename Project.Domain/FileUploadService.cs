using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;
using Project.Domain.Exceptions;
using Project.Generic;

namespace Project.Domain;
public class FileUploadService
{
    private readonly IConfiguration _configuration;
    private readonly ShareClient _shareClient;

    public FileUploadService(IConfiguration configuration)
    {
        _configuration = configuration;
        _shareClient = new ShareClient(_configuration.GetConnectionString("Azure:StorageAccount")!, _configuration.GetConnectionString("Azure:ShareName")!);
    }

    public async Task<IFileModel> UploadFile(Guid directory, FileUploadModel file)
    {
        if (file.Content is null)
        {
            throw new ValidationException("File content is required");
        }
        var directoryClient = await GetOrCreateDirectory(directory);

        file.CreateNewFileName();
        var fileClient = directoryClient.GetFileClient(file.NewFileName);

        var fileStream = file.GetContentAsStream()!;

        await fileClient.CreateAsync(fileStream.Length);
        var uploadResult = await fileClient.UploadAsync(fileStream);

        file.ContentHash = uploadResult.Value.ContentHash.GetBytesAsHexString();

        return file;
    }

    public async Task<IFileModel> DownloadFile(Guid directory, FileDownloadRequest request)
    {
        var directoryClient = await GetDirectory(directory);
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

    private async Task<ShareDirectoryClient> GetOrCreateDirectory(Guid directory)
    {
        ShareDirectoryClient directoryClient = _shareClient.GetDirectoryClient(directory.ToString());
        if (!await directoryClient.ExistsAsync())
        {
            await directoryClient.CreateAsync();
        }
        return directoryClient;
    }
    private async Task<ShareDirectoryClient> GetDirectory(Guid directory)
    {
        ShareDirectoryClient directoryClient = _shareClient.GetDirectoryClient(directory.ToString());
        if (!await directoryClient.ExistsAsync())
        {
            throw new InvalidOperationException($"Directory '{directory}' not found");
        }
        return directoryClient;
    }
}

public static class FileExtensions
{
    public static MemoryStream? GetContentAsStream(this IFileModel file)
    {
        var bytes = file.GetContentAsByteArray();
        return bytes is null ? null : new(bytes);
    }
    public static byte[]? GetContentAsByteArray(this IFileModel file) => file.Content?.GetStringAsBytes();

    public static string FullNewFileName(this IFileInfo info) => $"{info.NewFileName}.{info.Extension}";
    public static string FullOriginalFileName(this IFileInfo info) => $"{info.OriginalFileName}.{info.Extension}";
}

public interface IFileInfo
{
    string OriginalFileName { get; }
    string? NewFileName { get; }
    string Extension { get; }
}
public interface IFileModel : IFileInfo
{
    string? Content { get; }
    string? ContentHash { get; }
}

public class FileUploadModel : IFileModel
{
    public string OriginalFileName { get; set; } = null!;
    public string NewFileName { get; private set; } = null!;
    public string Extension { get; set; } = null!;
    public string? Content { get; set; }
    public string? ContentHash { get; set; }

    public void CreateNewFileName() => NewFileName = Guid.NewGuid().ToString();
}

public class FileDownloadRequest : IFileInfo
{
    public string NewFileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string Extension { get; set; } = null!;
}

public class FileDownloadModel : IFileModel
{
    public string OriginalFileName { get; set; } = null!;
    public string NewFileName { get; init; } = null!;
    public string Extension { get; init; } = null!;
    public string Content { get; set; } = null!;
    public string? ContentHash { get; set; }

    public void SetContentFromStream(Stream stream)
    {
        using MemoryStream memStream = new MemoryStream();
        stream.CopyTo(memStream);
        SetContentFromByteArray(memStream.ToArray());
    }
    public void SetContentFromByteArray(byte[] bytes)
    {
        Content = bytes.GetBytesAsString();
    }
}