using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Project.Domain.Exceptions;

namespace Project.Domain;

public interface IApplicationUser
{
    public string Id { get; }
    public string UserName { get; }
}
public class ApplicationUserModel : IApplicationUser
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
}

public interface ICurrentUserAccessor
{
    public void SetUser(IApplicationUser? user);
    public IApplicationUser? User { get; }
}

