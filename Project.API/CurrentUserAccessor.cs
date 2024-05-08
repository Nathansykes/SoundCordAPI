using Microsoft.AspNetCore.Identity;
using Project.Auth.Identity.Models;
using Project.Domain;

namespace Project.API;
public class CurrentUserAccessor(IServiceProvider serviceProvider) : ICurrentUserAccessor
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private IApplicationUser? _user;

    public IApplicationUser? User { get => _user ??= GetUser(); }

    private IApplicationUser? GetUser()
    {
        var httpContext = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
        if (httpContext is null)
            return null;
        return GetUser(httpContext, _serviceProvider);
    }
    public static IApplicationUser? GetUser(HttpContext context, IServiceProvider services)
    {
        var user = context.User;
        if (user is null || user.Identity?.Name is null || (!user.Identity.IsAuthenticated))
        {
            return null;
        }

        var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
        var appUser = new ApplicationUserModel
        {
            Id = userMgr.GetUserId(user)!,
            UserName = userMgr.GetUserName(user)!
        };
        return appUser;
    }
    public void SetUser(IApplicationUser? user)
    {
        _user = user;
    }
}
