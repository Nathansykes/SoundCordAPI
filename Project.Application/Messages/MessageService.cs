﻿using Project.Domain;
using Project.Domain.Messages;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Messages;
public class MessageService(IMessageRepository<Message> messageRepositor1y, IModelMapper<Message, MessageModel> mapper) : IMessageService
{
    private readonly IMessageRepository<Message> _messageRepository = messageRepositor1y;
    private readonly IModelMapper<Message, MessageModel> _mapper = mapper;

    public MessageModel CreateMessage(Guid channelId, MessageModel message)
    {
        message.ChannelId = channelId;
        var entity = _mapper.MapToDatabaseModel(message);
        _messageRepository.Create(channelId, entity);
        return _mapper.MapToDomainModel(entity);
    }

    public MessageModel GetMessage(Guid messageId)
    {
        var entity = _messageRepository.GetById(messageId);
        return _mapper.MapToDomainModel(entity);
    }

    public ICollection<MessageModel> GetMessages(Guid channelId)
    {
        var entities = _messageRepository.GetByChannelId(channelId).OrderByDescending(x => x.Utc).ToList();
        return entities.Select(x => _mapper.MapToDomainModel(x)).ToList();
    }

    public ICollection<MessageModel> GetMessagesForSongRevision(Guid channelId, Guid songRevisionId)
    {
        var entities = _messageRepository.GetByChannelId(channelId).Where(x => x.SongRevisionId == songRevisionId).ToList();
        return entities.Select(x => _mapper.MapToDomainModel(x)).ToList();
    }
}
