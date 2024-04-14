namespace Project.Domain.Files.FileUploading;

public class FileDownloadRequest : IFileInfo
{
    public string NewFileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string Extension { get; set; } = null!;
}
