using Microsoft.AspNetCore.SignalR;
using Project.Domain.Channels;
using Project.Domain.Messages;
using SignalRSwaggerGen.Attributes;
using System.Text.Json;

namespace Project.API.Hubs;

public class MessageHub(/*IChannelService channelService, IMessageService messageService*/) : Hub
{
    //private readonly IChannelService _channelService = channelService;
    //private readonly IMessageService _messageService = messageService;

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(Guid channelId, MessageModel message)
    {
        message.ChannelId = channelId;
        await Clients.All.SendAsync("message", message);
    }
}
