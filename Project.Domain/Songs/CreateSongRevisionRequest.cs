using Project.Domain.Files;

namespace Project.Domain.Songs;
public class CreateSongRevisionRequest
{
    public SongRevisionModel SongRevision { get; set; } = new();
    public FileModel File { get; set; } = new();
}
