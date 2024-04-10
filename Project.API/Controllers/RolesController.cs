using Project.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using Project.Auth.Roles.Models;
using Microsoft.AspNetCore.Identity;

namespace Project.API.Controllers;

[Route("api/[controller]/[action]")]
public class RolesController(RolesService rolesService) : BaseController
{
    private readonly RolesService _rolesService = rolesService;


    [HttpGet(Name = "")]
    public async Task<IActionResult> GetMyRoles()
    {
        return Ok(await _rolesService.GetUserRolesAsync(User.Identity!.Name!));
    }


    [HttpPost]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> Create([FromBody] RoleRequest role)
    {
        await _rolesService.CreateRoleAsync(role.RoleName);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> Delete([FromBody] RoleRequest role)
    {
        await _rolesService.DeleteRoleAsync(role.RoleName);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> RoleExists([FromBody] RoleRequest role)
    {
        return Ok(await _rolesService.RoleExistsAsync(role.RoleName));
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _rolesService.GetRolesAsync());
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRolesRequest request)
    {
        await _rolesService.AddUserToRoleAsync(request.UserName, request.Role);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> GetUserRoles([FromBody] UserRequest request)
    {
        return Ok(await _rolesService.GetUserRolesAsync(request.UserName));
    }
}
