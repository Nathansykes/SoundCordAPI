namespace Project.Domain.Groups;
public interface IGroupRepository<TGroupEntity> where TGroupEntity : new()
{
    void AddUserToGroup(Guid id, string userName);
    void RemoveUserFromGroup(Guid id, string userName);
    TGroupEntity Create(TGroupEntity entity);
    void DeleteById(Guid id);
    IQueryable<TGroupEntity> GetAll();
    TGroupEntity GetById(Guid id);
    ICollection<TGroupEntity> GetByName(string name);
    bool TryGetById(Guid id, out TGroupEntity? entity);
}