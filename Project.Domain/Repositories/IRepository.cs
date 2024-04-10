namespace Project.Domain.Repositories;
public interface IRepository<TModel, TId> where TModel : class, new()
{
    TModel Add(TModel model);
    IEnumerable<TModel> GetAll();
    TModel GetById(TId id);
    bool ExistsById(TId id);
    TModel Update(TModel model);
    void DeleteById(TId id);
}
