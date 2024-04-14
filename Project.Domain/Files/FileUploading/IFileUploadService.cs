
namespace Project.Domain.Files.FileUploading;

public interface IFileUploadService
{
    Task<IFileModel> DownloadFile(IEnumerable<Guid> directories, FileDownloadRequest request);
    Task<IFileModel> UploadFile(IEnumerable<Guid> directories, FileUploadModel file);
}