namespace Project.Auth.Identity;

public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizeAttribute() => base.AuthenticationSchemes = "Identity.Bearer";
    public AuthorizeAttribute(string policy) : base(policy) => base.AuthenticationSchemes = "Identity.Bearer";
    public new string? AuthenticationSchemes
    {
        get => "Identity.Bearer";
        set { }
    }
}
