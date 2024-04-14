namespace Project.Domain;
public interface IUserAccessValidator
{
    bool CreatedFileMetadata(Guid fileMetadatumId);
    bool CreatedGroup(Guid groupId);
    bool CreatedSong(Guid songId);
    bool CreatedSongRevision(Guid songRevisionId);
    bool HasAccessToChannel(Guid channelId);
    bool HasAccessToFile(Guid fileMetadatumId);
    bool HasAccessToGroup(Guid groupId);
    bool HasAccessToSong(Guid songId);
    bool HasAccessToSongRevision(Guid songRevisionId);
}
