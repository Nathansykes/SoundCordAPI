using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Messages;
public interface IMessageRepository<TMessageEntity> where TMessageEntity : new()
{
    TMessageEntity Create(Guid channelId, TMessageEntity entity);
    void DeleteById(Guid id);
    IQueryable<TMessageEntity> GetByChannelId(Guid channelId);
    TMessageEntity GetById(Guid id);
    bool TryGetById(Guid id, out TMessageEntity? entity);
}