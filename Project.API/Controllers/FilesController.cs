using Microsoft.AspNetCore.Mvc;
using Project.Domain.Files.FileUploading;

namespace Project.API.Controllers;

[Route("api/[controller]")]
public class FilesController(FileUploadService fileUploadService) : BaseController
{
    private readonly FileUploadService _fileUploadService = fileUploadService;

    [HttpPost("Upload")]
    public async Task<IActionResult> UploadFile(FileUploadModel model)
    {
        var directory = Guid.Parse("0497d7e0-54ab-4e76-98ab-746fbc8d8cef");
        var result = await _fileUploadService.UploadFile([directory], model);
        return Ok(result);
    }
    [HttpPost("Download")]
    public async Task<IActionResult> DownloadFile(FileDownloadRequest model)
    {
        var directory = Guid.Parse("0497d7e0-54ab-4e76-98ab-746fbc8d8cef");
        var result = await _fileUploadService.DownloadFile([directory], model);
        return Ok(result);
    }
}
