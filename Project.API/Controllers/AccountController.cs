using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Auth.Identity.Models;
using Project.Auth.Roles;
using Project.Domain;
using Project.Infrastructure.Model;

namespace Project.API.Controllers;

[Route("api/[controller]/[action]")]
public class AccountController(
    AuthorizationService authorizationService,
    IServiceProvider serviceProvider,
    RolesService rolesService,
    IApplicationDbContext context) : BaseController
{
    private readonly AuthorizationService _authorizationService = authorizationService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly RolesService _rolesService = rolesService;
    private readonly IApplicationDbContext _context = context;

    [HttpGet("~/api/account/user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userAccessor = _serviceProvider.GetRequiredService<ICurrentUserAccessor>();
        var user = userAccessor.User;
        var roles = await _rolesService.GetUserRolesAsync(user!.UserName);
        var model = new UserInfoModel(user.UserName, roles);
        return Ok(model);
    }


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

        var exampleGroupId = Guid.Parse("F8E94F19-6301-EF11-AAF0-6045BD13BBCF");
        
        var group = _context.Groups.FirstOrDefault(x => x.Id == exampleGroupId);
        var user = _context.AspNetUsers.FirstOrDefault(x => x.UserName == registration.Username);
        if (group is not null && user is not null)
        {
            group.Users.Add(user);
            _context.SaveChanges();
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
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authorizationService.ResetPasswordAsync(request);
        if (!result.Succeeded)
            return CreateValidationProblem(result);
        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var result = await _authorizationService.ForgotPasswordAsync(request);
        if (!result.Succeeded)
            return CreateValidationProblem(result);
        return Ok();
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
