using Project.Application.Groups;
using Project.Domain.Exceptions;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Repositories;

public class GroupRepository(IUserApplicationDbContext context) : IGroupRepository<Group>
{
    private readonly IUserApplicationDbContext _context = context;

    public Group Create(Group entity)
    {
        entity.CreatedByUserId = _context.ContextUser.Id;
        _context.Groups.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void DeleteById(Guid id)
    {
        var entity = _context.Groups.FirstOrDefault(x => x.Id == id && x.CreatedByUserId == _context.ContextUser.Id)
            ?? throw new NotFoundException($"Could not delete group {id} because it does not exists or the user did not create it");
        _context.Groups.Remove(entity);
        _context.SaveChanges();
    }

    public IQueryable<Group> GetAll()
    {
        return _context.Groups.Where(x => x.Users.Any(y => y.Id == _context.ContextUser.Id));
    }

    public Group GetById(Guid id)
    {
        return TryGetById(id, out var entity) ? entity! : throw new NotFoundException($"No group found for current user with Id: {id}");
    }


    public bool TryGetById(Guid id, out Group? entity)
    {
        entity = GetAll().FirstOrDefault(x => x.Id == id);
        return entity is not null;
    }

    public ICollection<Group> GetByName(string name)
    {
        return _context.ContextUser.Groups.Where(x => x.GroupName == name).ToList();
    }

    public void AddUserToGroup(Guid id, string userName)
    {
        var user = _context.AspNetUsers.FirstOrDefault(x => x.UserName == userName) ?? throw new NotFoundException($"No user found with username: {userName}");
        var group = GetById(id);
        if (group.Users.Any(x => x.UserName == userName))
            throw new ValidationException($"{userName} is already in group: {id}");
        group.Users.Add(user);
        _context.SaveChanges();
    }
}
