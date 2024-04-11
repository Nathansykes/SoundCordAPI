using Microsoft.AspNetCore.Mvc;
using Project.Domain.Channels;

namespace Project.API.Controllers;

[Route("api/Groups/{groupId}/[controller]")]
public class ChannelsController(IChannelService channelService) : BaseController
{
    private readonly IChannelService _channelService = channelService;

    [HttpGet]
    [ProducesResponseType(typeof(ChannelModel[]), StatusCodes.Status200OK)]
    public IActionResult GetChannels(Guid groupId)
    {
        var channels = _channelService.GetChannels(groupId);
        return Ok(channels);
    }

    [ProducesResponseType(typeof(ChannelModel), StatusCodes.Status200OK)]
    [Route("~/api/[controller]/{channelId}")]
    [HttpGet]
    public IActionResult GetChannel(Guid channelId)
    {
        var channel = _channelService.GetChannel(channelId);
        return Ok(channel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChannelModel), StatusCodes.Status200OK)]
    public IActionResult CreateChannel(Guid groupId, [FromBody] ChannelModel channel)
    {
        var createdChannel = _channelService.CreateChannel(groupId, channel);
        return Ok(createdChannel);
    }


}
