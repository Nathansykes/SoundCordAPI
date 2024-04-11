using Project.Domain;

namespace Project.Domain.Channels;
public interface IChannelRepository<TGroupEntity> : IRepository<TGroupEntity, Guid> where TGroupEntity : class, new()
{
    IQueryable<TGroupEntity> GetByGroupId(Guid groupId);
}
