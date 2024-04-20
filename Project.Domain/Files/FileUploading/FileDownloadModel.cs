using Project.Generic;

namespace Project.Domain.Files.FileUploading;

public class FileDownloadModel : IFileModel
{
    public string OriginalFileName { get; set; } = null!;
    public string NewFileName { get; init; } = null!;
    public string Extension { get; init; } = null!;
    public string Content { get; set; } = null!;
    public string? ContentHash { get; set; }
    public string? ContentType { get; set; }

    public void SetContentFromStream(Stream stream)
    {
        using MemoryStream memStream = new MemoryStream();
        stream.CopyTo(memStream);
        SetContentFromByteArray(memStream.ToArray());
    }
    public void SetContentFromByteArray(byte[] bytes)
    {
        Content = bytes.GetBytesAsBase64String();
    }
}