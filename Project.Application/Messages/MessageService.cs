using Project.Domain;
using Project.Domain.Messages;
using Project.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Messages;
public class MessageService(IMessageRepository<Message> messageRepositor1y, IModelMapper<Message, MessageModel> mapper) : IMessageService
{
    private readonly IMessageRepository<Message> _messageRepository = messageRepositor1y;
    private readonly IModelMapper<Message, MessageModel> _mapper = mapper;

    public MessageModel CreateMessage(Guid channelId, MessageModel message)
    {
        var entity = _mapper.MapToDatabaseModel(message);
        _messageRepository.Create(channelId, entity);
        return _mapper.MapToDomainModel(entity);
    }

    public MessageModel GetMessage(Guid messageId)
    {
        return _mapper.MapToDomainModel(_messageRepository.GetById(messageId));
    }

    public ICollection<MessageModel> GetMessages(Guid channelId)
    {
        var entities = _messageRepository.GetByChannelId(channelId).ToList();
        return entities.Select(x => _mapper.MapToDomainModel(x)).ToList();
    }
}
