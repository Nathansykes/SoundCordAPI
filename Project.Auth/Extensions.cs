using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Project.Auth.Identity.Models;
using System.Text;

namespace Project.Auth;
public static class Extensions
{

}
public class AuthorizationExtensions(UserManager<ApplicationUser> userManager)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    public async Task<ApplicationUser?> GetApplicationUserFromBasicAuthAsync(HttpContext context)
    {
        string? authHeader = context.Request.Headers.Authorization;
        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            var encodedCredentials = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials!));

            var email = decodedCredentials.Split(':', 2)[0];
            var password = decodedCredentials.Split(':', 2)[1];

            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null && await _userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
        }
        return null;
    }
}