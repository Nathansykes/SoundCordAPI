namespace Project.Domain.Songs;
public interface ISongRevisionService
{
    Task<SongRevisionModel> CreateSongRevision(Guid songId, CreateSongRevisionRequest request);
    SongRevisionModel GetSongRevisionById(Guid id);
    List<SongRevisionModel> GetSongRevisions(Guid songId);
    List<SongRevisionModel> GetSongRevisionsByChannelId(Guid channelId);
}
