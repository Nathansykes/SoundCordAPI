namespace Project.Domain.Channels;

public interface IChannelService
{
    bool UserHasAccessToChannel(Guid channelId);
    ChannelModel CreateChannel(Guid groupId, ChannelModel channel);
    ChannelModel GetChannel(Guid channelId);
    ICollection<ChannelModel> GetChannels(Guid groupId);
}


