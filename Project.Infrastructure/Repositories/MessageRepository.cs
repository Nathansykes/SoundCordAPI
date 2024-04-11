﻿using Project.Domain.Channels;
using Project.Domain.Exceptions;
using Project.Domain.Messages;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories;
public class MessageRepository(
    IUserApplicationDbContext context, 
    IChannelRepository<Channel> channelRepository) : IMessageRepository<Message>
{
    private readonly IUserApplicationDbContext _context = context;
    private readonly IChannelRepository<Channel> _channelRepository = channelRepository;

    public Message Create(Guid channelId, Message entity)
    {
        var channel = _channelRepository.GetById(channelId);

        entity.UserId = _context.ContextUser.Id;
        _context.Messages.Add(entity);
        channel.ChannelMessages.Add(new ChannelMessage
        {
            Message = entity
        });
        _context.SaveChanges();
        return entity;
    }

    public void DeleteById(Guid id)
    {
        var entity = _context.Messages.FirstOrDefault(x => x.Id == id && x.UserId == _context.ContextUser.Id)
            ?? throw new NotFoundException($"Could not delete message {id} because it does not exists or the user did not create it");
        _context.Messages.Remove(entity);
        _context.SaveChanges();
    }

    public IQueryable<Message> GetByChannelId(Guid channelId)
    {
        return _context.ChannelMessages.Where(x => x.ChannelId == channelId).Select(x => x.Message);
    }

    public Message GetById(Guid id)
    {
        return TryGetById(id, out Message? message) ? message! : throw new NotFoundException($"No message found for current user with Id: {id}");
    }

    public bool TryGetById(Guid id, out Message? entity)
    {
        entity = _context.Messages.FirstOrDefault(x => x.Id == id);
        if (entity is null || entity.ChannelMessage?.Channel.Group.Users.Any(y => y.Id == _context.ContextUser.Id) == false)
        {
            entity = null;
            return false;
        }
        return entity is not null;
    }
}