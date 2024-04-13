using Microsoft.AspNetCore.SignalR;
using Project.Domain.Channels;
using Project.Domain.Messages;
using Project.Infrastructure.Model.Entities;
using SignalRSwaggerGen.Attributes;
using System.Text.Json;

namespace Project.API.Hubs.Messages;

[Authorize]
[SignalRHub("/messageshub")]
public class MessageHub(IChannelService channelService, IMessageService messageService) : Hub
{
    private readonly IChannelService _channelService = channelService;
    private readonly IMessageService _messageService = messageService;

    public async Task Message(SendChannelMessageRequest request)
    {
        if (!await ValidateChannelAccess(request.ChannelId))
            return;

        var createdMessage = _messageService.CreateMessage(request.ChannelId, request.Message);
        await Clients.Group(request.ChannelId.ToString()).SendAsync("Message", createdMessage);
    }

    public async Task ConnectToChannel(ChannelHubRequest request)
    {
        if (!await ValidateChannelAccess(request.ChannelId))
            return;

        await Groups.AddToGroupAsync(Context.ConnectionId, request.ChannelId.ToString());
        await Clients.Caller.SendAsync("Connected", request.ChannelId);
    }

    public async Task DisconnectFromChannel(ChannelHubRequest request)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, request.ChannelId.ToString());
        await Clients.Caller.SendAsync("Disconnected", request.ChannelId);
    }


    private async Task<bool> ValidateChannelAccess(Guid channelId)
    {
        if (!_channelService.UserHasAccessToChannel(channelId))
        {
            await Clients.Caller.SendAsync("Error", "You do not have access to this channel");
            return false;
        }
        return true;
    }

}