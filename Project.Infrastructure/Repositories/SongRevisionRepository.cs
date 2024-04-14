using Project.Domain;
using Project.Domain.Exceptions;
using Project.Domain.Songs;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Repositories;
public class SongRevisionRepository(
    IUserApplicationDbContext context,
    ISongRepository<Song> songRepository,
    IUserAccessValidator userAccessValidator) : ISongRevisionRepository<SongRevision>
{
    private readonly IUserApplicationDbContext _context = context;
    private readonly ISongRepository<Song> _songRepository = songRepository;
    private readonly IUserAccessValidator _userAccessValidator = userAccessValidator;

    public SongRevision Create(SongRevision entity)
    {
        if (!_userAccessValidator.HasAccessToSong(entity.SongId))
            throw new DataAccessException("User does not have access to song");
        _context.SongRevisions.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void DeleteById(Guid id)
    {
        var entity = GetById(id);
        _context.SongRevisions.Remove(entity);
        _context.SaveChanges();
    }

    public IQueryable<SongRevision> GetAll()
    {
        return _songRepository.GetAll().SelectMany(x => x.SongRevisions);
    }

    public IQueryable<SongRevision> GetBySongId(Guid songId)
    {
        return GetAll().Where(x => x.SongId == songId);
    }

    public IQueryable<SongRevision> GetByChannelId(Guid channelId)
    {
        return _context.SongRevisions.Where(x => x.Song.ChannelId == channelId);
    }

    public SongRevision GetById(Guid id)
    {
        return TryGetById(id, out var entity) ? entity! : throw new NotFoundException($"No SongRevision found with Id: {id}");
    }

    public bool TryGetById(Guid id, out SongRevision? entity)
    {
        entity = GetAll().FirstOrDefault(x => x.Id == id);
        return entity != null;
    }
}
