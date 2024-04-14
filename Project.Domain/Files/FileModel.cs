using Project.Domain.Files.FileUploading;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.Domain.Files;

public class FileModel : IFileModel
{
    public string FileName { get; set; } = "";
    public string Extension { get; set; } = "";
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedByUser { get; set; } = "";
    public string Content { get; set; } = "";
    [SwaggerSchema(ReadOnly = true)]
    public int ContentLength { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public string ContentType { get; set; } = "";

    string? IFileModel.ContentHash { get; }
    string IFileInfo.OriginalFileName => FileName;
    string? IFileInfo.NewFileName => FileName;
}