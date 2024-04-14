using Microsoft.AspNetCore.Mvc;
using Project.Domain.Files;
using Project.Domain.Files.FileUploading;
using Project.Infrastructure.Model.Entities;

namespace Project.API.Controllers;

[Route("api/[controller]")]
public class FilesController(FileUploadService fileUploadService, IFileMetadataRepository<FileMetadatum> fileMetadataRepository) : BaseController
{
    private readonly FileUploadService _fileUploadService = fileUploadService;
    private readonly IFileMetadataRepository<FileMetadatum> _fileMetadataRepository = fileMetadataRepository;

    //[HttpPost("Upload")]
    //public async Task<IActionResult> UploadFile(FileUploadModel model)
    //{
    //    var directory = Guid.Parse("0497d7e0-54ab-4e76-98ab-746fbc8d8cef");
    //    var result = await _fileUploadService.UploadFile([directory], model);
    //    return Ok(result);
    //}
    [HttpPost("Download{fileId}")]
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
        return Ok(result);
    }
}