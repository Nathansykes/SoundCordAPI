namespace Project.Domain.Files.FileUploading;

public interface IFileInfo
{
    string OriginalFileName { get; }
    string? NewFileName { get; }
    string Extension { get; }
}
