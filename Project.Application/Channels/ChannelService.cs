using Project.Domain;
using Project.Domain.Channels;
using Project.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Channels;
public class ChannelService(
    IChannelRepository<Channel> channelRepository,
    IModelMapper<Channel,ChannelModel> mapper) : IChannelService
{
    private readonly IChannelRepository<Channel> _channelRepository = channelRepository;
    private readonly IModelMapper<Channel,ChannelModel> _mapper = mapper;

    public ChannelModel CreateChannel(Guid groupId, ChannelModel channel)
    {
        channel.GroupId = groupId;
        var entity = _mapper.MapToDatabaseModel(channel);
        _channelRepository.Create(entity);
        return _mapper.MapToDomainModel(entity);
    }

    public ChannelModel GetChannel(Guid channelId)
    {
        var entity = _channelRepository.GetById(channelId);
        return _mapper.MapToDomainModel(entity);
    }

    public ICollection<ChannelModel> GetChannels(Guid groupId)
    {
        var entities = _channelRepository.GetByGroupId(groupId).ToList();
        return entities.Select(x => _mapper.MapToDomainModel(x)).ToList();
    }
}
