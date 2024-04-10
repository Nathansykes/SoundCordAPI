using Project.Auth.Identity;
using Project.Auth.Identity.Models;
using Project.Auth.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Project.API.Controllers;

[Route("api/[controller]/[action]")]
public class AccountController(AuthorizationService authorizationService) : BaseController
{
    private readonly AuthorizationService _authorizationService = authorizationService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registration)
    {
        var result = await _authorizationService.Register(registration);
        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }
        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var result = await _authorizationService.Login(login);
        if (!result.Succeeded)
            return CreateValidationProblem(result);
        return Empty;
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _authorizationService.ChangePasswordAsync(request);
        if (!result.Succeeded)
            return CreateValidationProblem(result);
        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var (identityResult, actionResult) = await _authorizationService.RefreshAsync(request);
        if (!identityResult.Succeeded)
            return CreateValidationProblem(identityResult);

        await actionResult!.ExecuteAsync(HttpContext);
        return Empty;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.GlobalAdmin)]
    public async Task<IActionResult> AdminChangePassword([FromBody] AdminChangePasswordRequest request)
    {
        var result = await _authorizationService.AdminChangePasswordAsync(request);
        if (!result.Succeeded)
            return CreateValidationProblem(result);
        return Ok();
    }
    private ActionResult CreateValidationProblem(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            if (error.Code.Contains("password"))
                ModelState.AddModelError("password", error.Description);
            else if (error.Code.Contains("email", StringComparison.OrdinalIgnoreCase)
                    || error.Code.Contains("username", StringComparison.OrdinalIgnoreCase))
                ModelState.AddModelError("email", error.Description);
            else
                ModelState.AddModelError(error.Code, error.Description);
        }
        
        return ValidationProblem(ModelState);
    }
}
