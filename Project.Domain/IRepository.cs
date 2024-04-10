namespace Project.Domain;
public interface IRepository<TEntity, TId> where TEntity : class, new()
{
    TEntity Create(TEntity entity);
    IQueryable<TEntity> GetAll();
    bool TryGetById(TId id, out TEntity? entity);
    TEntity GetById(TId id);
    void DeleteById(TId id);
}
