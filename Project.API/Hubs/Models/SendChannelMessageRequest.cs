using Project.Domain.Messages;

namespace Project.API.Hubs.Models;

public class SendChannelMessageRequest : IHubRequest
{
    public Guid ChannelId { get; set; }
    public MessageModel Message { get; set; } = null!;
}