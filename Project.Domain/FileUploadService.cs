using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares.Specialized;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Project.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;

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

    public async Task<FileUploadResult> UploadFile(Guid directory, FileUploadRequest request)
    {
        var t = _shareClient.Exists();
        var directoryClient = await GetOrCreateDirectory(directory);

        request.CreateNewFileName();
        var fileClient = directoryClient.GetFileClient(request.FullNewFileName);
        
        var fileStream = request.File.GetContentAsStream();

        await fileClient.CreateAsync(fileStream.Length);
        var uploadResult = await fileClient.UploadAsync(fileStream);

        var result = new FileUploadResult
        {
            File = request.File,
            NewFileName = request.NewFileName,
            ContentHash = uploadResult.Value.ContentHash.GetBytesAsHexString()
        };
        return result;
    }

    public async Task<ShareDirectoryClient> GetOrCreateDirectory(Guid directory)
    {
        ShareDirectoryClient directoryClient = _shareClient.GetDirectoryClient(directory.ToString());
        if (!await directoryClient.ExistsAsync())
        {
            await directoryClient.CreateAsync();
        }
        return directoryClient;
    }
}

public class FileModel
{
    public string FileName { get; set; } = null!;
    public string Extension { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Stream GetContentAsStream() => new MemoryStream(GetContentAsByteArray());
    public byte[] GetContentAsByteArray() => Content.GetStringAsBytes();
    public string FullFileName => $"{FileName}.{Extension}";

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

public class FileUploadRequest
{
    public FileModel File { get; set; } = null!;
    public string NewFileName { get; private set; } = null!;
    public string FullNewFileName => $"{NewFileName}.{File.Extension}";
    public string CreateNewFileName() => NewFileName = Guid.NewGuid().ToString();
}

public class FileUploadResult
{
    public FileModel File { get; set; } = null!;
    public string NewFileName { get; set; } = null!;
    public string ContentHash { get; set; } = null!;
    public string Extension => File.Extension;
    public string FullNewFileName => $"{NewFileName}.{Extension}";
}