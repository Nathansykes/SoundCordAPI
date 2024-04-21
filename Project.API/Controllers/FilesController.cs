using Microsoft.AspNetCore.Mvc;
using Project.Domain.Files;
using Project.Domain.Files.FileUploading;
using Project.Infrastructure.Model.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Project.API.Controllers;

[Route("api/[controller]")]
public class FilesController(IFileUploadService fileUploadService, IFileMetadataRepository<FileMetadatum> fileMetadataRepository) : BaseController
{
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly IFileMetadataRepository<FileMetadatum> _fileMetadataRepository = fileMetadataRepository;


    [HttpPost("download/{fileId}/meta")]
    public IActionResult DownloadFileMeta(Guid fileId)
    {
        var file = _fileMetadataRepository.GetById(fileId);

        var dl = new FileModel()
        {
            Id = file.Id,
            FileName = file.OriginalFileName,
            Extension = file.OriginalExtension,
            UploadedByUser = file.UploadedByUser.UserName!,
            ContentLength = file.ContentLengthBytes,
            ContentType = file.ContentType,
        };
        return Ok(dl);
    }

    [HttpPost("download/{fileId}/content")]
    public async Task<FileContentResult> DownloadFileData(Guid fileId)
    {
        var file = _fileMetadataRepository.GetById(fileId);

        var directories = file.Directory.Split('/').Select(Guid.Parse);
        var request = new FileDownloadRequest()
        {
            Extension = file.OriginalExtension,
            NewFileName = file.NewFileName,
            OriginalFileName = file.OriginalFileName,
        };

        var result = await _fileUploadService.DownloadFile(directories, request);

        return File(result.GetContentAsByteArray()!, file.ContentType ?? "application/octet-stream");
    }


    [HttpPost("download/{fileId}")]
    [Experimental("api")]
    public async Task<IActionResult> DownloadFile(Guid fileId)
    {
        var file = _fileMetadataRepository.GetById(fileId);

        var directories = file.Directory.Split('/').Select(Guid.Parse);
        var request = new FileDownloadRequest()
        {
            Extension = file.OriginalExtension,
            NewFileName = file.NewFileName,
            OriginalFileName = file.OriginalFileName,
        };

        var result = await _fileUploadService.DownloadFile(directories, request);
        var dl = new FileModel()
        {
            Id = file.Id,
            Content = result.Content!,
            FileName = file.OriginalFileName,
            Extension = file.OriginalExtension,
            UploadedByUser = file.UploadedByUser.UserName!,
            ContentLength = file.ContentLengthBytes,
            ContentType = file.ContentType,
        };
        return Ok(dl);
    }
}