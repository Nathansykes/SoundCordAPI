namespace Project.Domain.Songs;
public interface ISongRepository<TSongEntity> where TSongEntity : new()
{
    TSongEntity Create(TSongEntity entity);
    void DeleteById(Guid id);
    IQueryable<TSongEntity> GetAll();
    IQueryable<TSongEntity> GetByGroupId(Guid groupId);
    TSongEntity GetById(Guid id);
    TSongEntity? GetForChannelId(Guid channelId);
    bool TryGetById(Guid id, out TSongEntity? entity);
}
