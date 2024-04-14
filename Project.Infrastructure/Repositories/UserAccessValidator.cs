using Project.Domain;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Repositories;
public class UserAccessValidator(IUserApplicationDbContext context) : IUserAccessValidator
{
    private readonly IUserApplicationDbContext _context = context;
    private IQueryable<Group> UserGroups => _context.Groups.Where(g => g.Users.Any(u => u.Id == _context.ContextUser.Id));

    public bool HasAccessToGroup(Guid groupId)
    {
        return UserGroups.Any(g => g.Id == groupId);
    }
    public bool HasAccessToChannel(Guid channelId)
    {
        return UserGroups.Any(g => g.Channels.Any(c => c.Id == channelId));
    }
    public bool HasAccessToSong(Guid songId)
    {
        return UserGroups.SelectMany(c => c.Channels).Any(c => c.Song != null && c.Song.Id == songId);
    }
    public bool HasAccessToSongRevision(Guid songRevisionId)
    {
        var createdByUser = _context.SongRevisions.Any(sr => sr.Id == songRevisionId && sr.CreatedByUserId == _context.ContextUser.Id);
        if (createdByUser)
            return true;

        return UserGroups.SelectMany(c => c.Channels)
            .Where(c => c.Song != null)
            .SelectMany(c => c.Song!.SongRevisions)
            .Any(sr => sr.Id == songRevisionId);
    }
    public bool HasAccessToFile(Guid fileMetadatumId)
    {
        var uploadedByUser = _context.FileMetadata.Any(fm => fm.Id == fileMetadatumId && fm.UploadedByUserId == _context.ContextUser.Id);
        if (uploadedByUser)
            return true;

        return UserGroups.SelectMany(c => c.Channels)
            .Where(c => c.Song != null)
            .SelectMany(c => c.Song!.SongRevisions)
            .Any(sr => sr.FileMetaDataId == fileMetadatumId);
    }

    public bool CreatedGroup(Guid groupId)
    {
        return _context.Groups.Any(g => g.Id == groupId && g.CreatedByUserId == _context.ContextUser.Id);
    }

    public bool CreatedSong(Guid songId)
    {
        return _context.Songs.Any(g => g.Id == songId && g.CreatedByUserId == _context.ContextUser.Id);
    }
    public bool CreatedSongRevision(Guid songRevisionId)
    {
        return _context.SongRevisions.Any(g => g.Id == songRevisionId && g.CreatedByUserId == _context.ContextUser.Id);
    }
    public bool CreatedFileMetadata(Guid fileMetadatumId)
    {
        return _context.FileMetadata.Any(g => g.Id == fileMetadatumId && g.UploadedByUserId == _context.ContextUser.Id);
    }
}
