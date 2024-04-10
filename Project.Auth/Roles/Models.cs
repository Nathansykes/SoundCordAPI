namespace Project.Auth.Roles.Models;

public record AddUserToRolesRequest(string UserName, string Role);
public record RoleRequest(string RoleName);
public record UserRequest(string UserName);