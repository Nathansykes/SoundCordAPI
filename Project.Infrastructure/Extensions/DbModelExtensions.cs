using Project.Domain.Files.FileUploading;

namespace Project.Infrastructure.Model.Entities;

public partial class FileMetadatum : IFileModel
{
    string? IFileModel.Content { get; }
    string IFileInfo.Extension => OriginalExtension;
}