namespace Project.API.Hubs.Models;

public class ConnectToChannelRequest : IHubRequest
{
    public Guid ChannelId { get; set; }
}