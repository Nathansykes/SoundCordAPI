using Project.Domain.Messages;

namespace Project.API.Hubs.Messages;

public class SendChannelMessageRequest : ChannelHubRequest
{
    public MessageModel Message { get; set; } = null!;
}