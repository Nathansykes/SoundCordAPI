using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Project.Auth.Identity.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.Auth.Identity;
public class AuthorizationService(
    IUserStore<ApplicationUser> userStore,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions)
{
    private readonly IUserStore<ApplicationUser> _userStore = userStore;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions = bearerTokenOptions;

    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    public async Task<IdentityResult> Register(RegisterRequest registration)
    {
        var email = registration.Username;
        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(email));
        }

        var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
        var user = new ApplicationUser();
        await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, registration.Password);

        return result;
    }
    public async Task<IdentityResult> Login(LoginRequest login)
    {
        _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var user = await _userManager.FindByNameAsync(login.Username);
        if (user is null)
            return IdentityResult.Failed(_userManager.ErrorDescriber.UsernameNotFound(login.Username));

        var signInResult = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
                return IdentityResult.Failed(IdentityErrors.LockedOut);
            if (signInResult.IsNotAllowed)
                return IdentityResult.Failed(IdentityErrors.NotAllowed);
            if (signInResult.RequiresTwoFactor)
                return IdentityResult.Failed(IdentityErrors.RequiresTwoFactor);
            return IdentityResult.Failed(IdentityErrors.IncorrectPassword);
        }

        return IdentityResult.Success;
    }
    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return IdentityResult.Failed(_userManager.ErrorDescriber.InvalidUserName(request.Username));

        var passwordCorrect = await _userManager.CheckPasswordAsync(user, request.OldPassword);
        if (!passwordCorrect)
            return IdentityResult.Failed(IdentityErrors.IncorrectPassword);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        return result;
    }
    public async Task<(IdentityResult IdentityResult, IResult? ActionResult)> RefreshAsync(RefreshRequest refreshRequest)
    {
        var refreshTokenProtector = _bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                DateTime.UtcNow >= expiresUtc ||
                await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not ApplicationUser user)
        {
            return (IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken()), null);
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user)!;
        var newUser = await _userManager.GetUserAsync(newPrincipal);
        if (newUser is null)
            return (IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken()), null);

        //await _signInManager.SignInAsync(user, refreshTicket.Properties, IdentityConstants.BearerScheme);
        //await _signInManager.SignInAsync((ApplicationUser)newPrincipal, isPersistent: false, IdentityConstants.BearerScheme);
        IResult result = TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);

        return (IdentityResult.Success, result);
    }
    public async Task<IdentityResult> AdminChangePasswordAsync(AdminChangePasswordRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return IdentityResult.Failed(_userManager.ErrorDescriber.UsernameNotFound(request.Username));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        return result;
    }
}

public static class IdentityErrors
{
    public static IdentityError UsernameNotFound(this IdentityErrorDescriber describer, string username) => new() { Code = "UsernameNotFound", Description = $"Username {username} not found" };
    public static IdentityError LockedOut => new() { Code = "LockedOut", Description = "This user is locked out, contact an admin to resolve" };
    public static IdentityError NotAllowed => new() { Code = "NotAllowed", Description = "This user is not allowed to sign in" };
    public static IdentityError RequiresTwoFactor => new() { Code = "RequiresTwoFactor", Description = "This user requires two factor authentication" };
    public static IdentityError IncorrectPassword => new() { Code = "IncorrectPassword", Description = "Password is incorrect" };
}