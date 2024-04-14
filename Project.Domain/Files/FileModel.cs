using Project.Domain.Files.FileUploading;

namespace Project.Domain.Files;

public class FileModel : IFileModel
{
    public string FileName { get; set; } = "";
    public string Extension { get; set; } = "";
    public string CreatedByUser { get; set; } = "";
    public string Content { get; set; } = "";
    public int ContentLength { get; set; }
    public string ContentType { get; set; } = "";

    string? IFileModel.ContentHash { get; }
    string IFileInfo.OriginalFileName => FileName;
    string? IFileInfo.NewFileName => FileName;
}