using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public IApplicationUser User { get; }
}

public class CurrentUserAccessor(IApplicationUser user) : ICurrentUserAccessor
{
    public IApplicationUser User { get; } = user;
}