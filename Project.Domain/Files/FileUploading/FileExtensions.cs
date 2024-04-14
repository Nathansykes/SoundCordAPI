using Project.Generic;

namespace Project.Domain.Files.FileUploading;

public static class FileExtensions
{
    public static MemoryStream? GetContentAsStream(this IFileModel file)
    {
        var bytes = file.GetContentAsByteArray();
        return bytes is null ? null : new(bytes);
    }
    public static byte[]? GetContentAsByteArray(this IFileModel file) => file.Content?.GetStringAsBytes();

    public static string FullNewFileName(this IFileInfo info) => $"{info.NewFileName}.{info.Extension}";
    public static string FullOriginalFileName(this IFileInfo info) => $"{info.OriginalFileName}.{info.Extension}";
}
