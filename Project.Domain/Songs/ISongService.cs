namespace Project.Domain.Songs;
public interface ISongService
{
    SongModel CreateSong(Guid groupId, SongModel song);
    List<SongModel> GetAllSongs();
    SongModel? GetForChannelId(Guid channelId);
    SongModel GetSongById(Guid id);
}
