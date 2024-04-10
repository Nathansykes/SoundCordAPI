using Project.Domain;

namespace Project.Application.Groups;
public interface IGroupRepository<TGroupEntity> : IRepository<TGroupEntity, Guid> where TGroupEntity : class, new()
{
    ICollection<TGroupEntity> GetByName(string name);
    void AddUserToGroup(Guid id, string userName);
}
