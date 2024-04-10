namespace Project.Auth.Roles.Models;

public record AddUserToRolesRequest(string Email, string Role);
public record RoleRequest(string RoleName);
public record UserRequest(string Email);