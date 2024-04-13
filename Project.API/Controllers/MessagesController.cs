using Microsoft.AspNetCore.Mvc;
using Project.Domain.Messages;

namespace Project.API.Controllers;

[Route("api/[controller]/")]
public class MessagesController(IMessageService MessageService) : BaseController
{
    private readonly IMessageService _messageService = MessageService;

    [HttpGet("{channelId}")]
    [ProducesResponseType(typeof(MessageModel[]), StatusCodes.Status200OK)]
    public IActionResult GetMessages(Guid channelId)
    {
        //TODO add pagination
        var messages = _messageService.GetMessages(channelId);
        return Ok(messages);
    }
}
