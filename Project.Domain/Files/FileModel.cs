using Project.Domain.Files.FileUploading;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.Domain.Files;

public class FileModel : IFileModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }
    public string FileName { get; set; } = "";
    public string Extension { get; set; } = "";
    public string? ContentType { get; set; } = "";
    public string Content { get; set; } = "";

    [SwaggerSchema(ReadOnly = true)]
    public string UploadedByUser { get; set; } = "";
    [SwaggerSchema(ReadOnly = true)]
    public DateTime UploadedUtc { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public int ContentLength { get; set; }

    string? IFileModel.ContentHash { get; }
    string IFileInfo.OriginalFileName => FileName;
    string? IFileInfo.NewFileName => FileName;
}