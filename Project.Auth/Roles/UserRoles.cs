namespace Project.Auth.Roles;
public static class UserRoles
{
    public const string Admin = "Admin";
    public const string GlobalAdmin = "GlobalAdmin";
    public const string Developer = "Developer";

    public static string Combine(params string[] roles) => string.Join(",", roles);
}
