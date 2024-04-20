namespace Project.Domain.Files.FileUploading;

public interface IFileModel : IFileInfo
{
    string? Content { get; }
    string? ContentHash { get; }
    string? ContentType { get; }
}
