namespace Project.Domain.Songs;
public interface ISongRevisionRepository<TSongRevisionEntity> where TSongRevisionEntity : new()
{
    TSongRevisionEntity Create(TSongRevisionEntity entity);
    void DeleteById(Guid id);
    IQueryable<TSongRevisionEntity> GetAll();
    IQueryable<TSongRevisionEntity> GetBySongId(Guid songId);
    IQueryable<TSongRevisionEntity> GetByChannelId(Guid channelId);
    TSongRevisionEntity GetById(Guid id);
    bool TryGetById(Guid id, out TSongRevisionEntity? entity);
}
