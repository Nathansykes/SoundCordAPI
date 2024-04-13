using Project.Domain.Channels;
using Project.Domain.Exceptions;
using Project.Domain.Songs;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Repositories;
public class SongRepository(IUserApplicationDbContext context, IChannelRepository<Channel> channelRepository) : ISongRepository<Song>
{
    private readonly IUserApplicationDbContext _context = context;
    private readonly IChannelRepository<Channel> _channelRepository = channelRepository;

    public Song Create(Song entity)
    {
        entity.CreatedByUserId = _context.ContextUser.Id;
        _context.Songs.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void DeleteById(Guid id)
    {
        var entity = GetById(id);
        _context.Songs.Remove(entity);
        _context.SaveChanges();
    }

    public IQueryable<Song> GetAll()
    {
        return _channelRepository.GetAll().Where(x => x.Song != null).Select(x => x.Song!);
    }

    public IQueryable<Song> GetByGroupId(Guid groupId)
    {
        return _context.Songs.Where(s => s.GroupId == groupId);
    }

    public Song GetById(Guid id)
    {
        return TryGetById(id, out Song? entity) ? entity! : throw new NotFoundException($"No song found with Id: {id}");
    }

    public Song? GetForChannelId(Guid channelId)
    {
        return _context.Songs.FirstOrDefault(s => s.ChannelId == channelId);
    }

    public bool TryGetById(Guid id, out Song? entity)
    {
        entity = GetAll().FirstOrDefault(x => x.Id == id);
        return entity != null;
    }
}
