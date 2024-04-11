using Project.Domain;
using System.Threading.Channels;

namespace Project.Domain.Channels;
public interface IChannelRepository<TChannelEntity> where TChannelEntity : new()
{
    TChannelEntity Create(TChannelEntity entity);
    void DeleteById(Guid id);
    IQueryable<TChannelEntity> GetAll();
    IQueryable<TChannelEntity> GetByGroupId(Guid groupId);
    TChannelEntity GetById(Guid id);
    bool TryGetById(Guid id, out TChannelEntity? entity);
}
