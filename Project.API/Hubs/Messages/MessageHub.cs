using Microsoft.AspNetCore.SignalR;
using Project.Domain;
using Project.Domain.Channels;
using Project.Domain.Messages;
using SignalRSwaggerGen.Attributes;

namespace Project.API.Hubs.Messages;

[Authorize]
[SignalRHub("/messageshub")]
public class MessageHub(IChannelService channelService, IMessageService messageService, ICurrentUserAccessor currentUserAccessor) : Hub
{
    private readonly IChannelService _channelService = channelService;
    private readonly IMessageService _messageService = messageService;
    private readonly ICurrentUserAccessor _currentUserAccessor = currentUserAccessor;

    private void SetUser()
    {
        if (_currentUserAccessor.User is null)
        {
            var user = new ApplicationUserModel
            {
                Id = Context.UserIdentifier!,
                UserName = Context.User!.Identity!.Name!
            };
            _currentUserAccessor.SetUser(user);
        }
    }

    public async Task Message(SendChannelMessageRequest request)
    {
        SetUser();
        if (!await ValidateChannelAccess(request.ChannelId))
            return;

        var createdMessage = _messageService.CreateMessage(request.ChannelId, request.Message);
        await Clients.Group(request.ChannelId.ToString()).SendAsync("Message", createdMessage);
    }

    public async Task Echo(string name, string message)
    {

        await Clients.Client(Context.ConnectionId).SendAsync("echo", name, $"{message} (echo from server)");
    }

    public async Task ConnectToChannel(ChannelHubRequest request)
    {
        SetUser();
        if (!await ValidateChannelAccess(request.ChannelId))
            return;

        await Groups.AddToGroupAsync(Context.ConnectionId, request.ChannelId.ToString());
        await Clients.Caller.SendAsync("Connected", request.ChannelId);
    }

    public async Task DisconnectFromChannel(ChannelHubRequest request)
    {
        SetUser();
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