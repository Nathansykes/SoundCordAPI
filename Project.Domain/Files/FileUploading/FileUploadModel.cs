namespace Project.Domain.Files.FileUploading;

public class FileUploadModel : IFileModel
{
    public string OriginalFileName { get; set; } = null!;
    public string NewFileName { get; private set; } = null!;
    public string Extension { get; set; } = null!;
    public string? Content { get; set; }
    public string? ContentHash { get; set; }

    public void CreateNewFileName() => NewFileName = Guid.NewGuid().ToString();
}
