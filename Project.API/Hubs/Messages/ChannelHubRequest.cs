namespace Project.API.Hubs.Messages;

public class ChannelHubRequest : IHubRequest
{
    public Guid ChannelId { get; set; }
}