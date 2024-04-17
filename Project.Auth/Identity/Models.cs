using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Project.Auth.Identity.Models;

public class ApplicationUser : IdentityUser
{

}

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) :
        IdentityDbContext<ApplicationUser>(options)
{

}


public record ChangePasswordRequest(string Username, string OldPassword, string NewPassword);
public record AdminChangePasswordRequest(string Username, string NewPassword);
public record RegisterRequest(string Username, [EmailAddress] string Email, string Password);
public record LoginRequest(string Username, string Password);
public record RefreshRequest(string RefreshToken);

public record ResetPasswordRequest(string Username, string Token, string NewPassword);
public record ForgotPasswordRequest(string Username);
