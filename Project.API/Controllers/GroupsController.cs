using Microsoft.AspNetCore.Mvc;
using Project.Domain.Groups;

namespace Project.API.Controllers;

[Route("api/[controller]")]
public class GroupsController(IGroupService groupService) : BaseController
{
    private readonly IGroupService _groupService = groupService;

    [HttpGet]
    [ProducesResponseType(typeof(GroupModel[]), StatusCodes.Status200OK)]
    public IActionResult GetGroups()
    {
        var groups = _groupService.GetGroups();
        return Ok(groups);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GroupModel), StatusCodes.Status200OK)]
    public IActionResult GetGroup(Guid id)
    {
        var group = _groupService.GetGroup(id);
        return Ok(group);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroupModel), StatusCodes.Status200OK)]
    public IActionResult CreateGroup([FromBody] GroupModel group)
    {
        var createdGroup = _groupService.CreateGroup(group);
        return Ok(createdGroup);
    }

    [HttpPost("{groupId}/users/{userName}")]
    public IActionResult AddUserToGroup(Guid groupId, string userName)
    {
        _groupService.AddUserToGroup(groupId, userName);
        return Ok();
    }

}
