using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Auth.Identity.Models;

public class ApplicationUser : IdentityUser
{

}

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) :
        IdentityDbContext<ApplicationUser>(options)
{

}


public record ChangePasswordRequest([EmailAddress] string Username, string OldPassword, string NewPassword);
public record AdminChangePasswordRequest([EmailAddress] string Username, string NewPassword);
public record RegisterRequest([EmailAddress] string Username, string Password);
public record LoginRequest([EmailAddress] string Username, string Password);
public record RefreshRequest(string RefreshToken);