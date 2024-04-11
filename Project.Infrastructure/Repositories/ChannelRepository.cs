using Project.Application.Groups;
using Project.Domain;
using Project.Domain.Channels;
using Project.Domain.Exceptions;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Repositories;

public class ChannelRepository(IUserApplicationDbContext context) : IChannelRepository<Channel>
{
    private readonly IUserApplicationDbContext _context = context;

    public Channel Create(Channel entity)
    {
        _context.Channels.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public IQueryable<Channel> GetAll()
    {
        return _context.Groups.Where(x => x.Users.Any(y => y.Id == _context.ContextUser.Id)).SelectMany(x => x.Channels);
    }

    public bool TryGetById(Guid id, out Channel? entity)
    {
        entity = _context.Channels.FirstOrDefault(x => x.Id == id);
        if (entity is null || (!entity.Group.Users.Any(x => x.Id == _context.ContextUser.Id)))
        {
            entity = null;
            return false;
        }
        return true;
    }

    public Channel GetById(Guid id)
    {
        return TryGetById(id, out Channel? channel) ? channel! : throw new NotFoundException("");
    }

    public IQueryable<Channel> GetByGroupId(Guid groupId)
    {
        return GetAll().Where(x => x.GroupId == groupId);
    }


    public void DeleteById(Guid id)
    {
        var entity = _context.Groups.FirstOrDefault(x => x.Id == id && x.CreatedByUserId == _context.ContextUser.Id)?.Channels.FirstOrDefault(x => x.Id == id)
            ?? throw new NotFoundException($"Could not delete group {id} because it does not exists or the user did not create it");
        _context.Channels.Remove(entity);
        _context.SaveChanges();
    }
}
