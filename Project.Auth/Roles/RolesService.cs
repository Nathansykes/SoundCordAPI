﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Auth.Identity.Models;

namespace Project.Auth.Roles;
public class RolesService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    public async Task CreateRoleAsync(string role)
    {
        await _roleManager.CreateAsync(new IdentityRole(role));
    }
    public async Task DeleteRoleAsync(string role)
    {
        var identityRole = await _roleManager.FindByNameAsync(role);
        if (identityRole is not null)
            await _roleManager.DeleteAsync(identityRole);
    }
    public async Task<bool> RoleExistsAsync(string role)
    {
        return await _roleManager.RoleExistsAsync(role);
    }
    public async Task<List<String>> GetRolesAsync()
    {
        return (await _roleManager.Roles.ToListAsync()).Select(x => x.Name!).ToList();
    }
    public async Task AddUserToRoleAsync(string userName, string role)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return;
        await _userManager.AddToRoleAsync(user, role);
    }
    public async Task RemoveUserFromRoleAsync(string userName, string role)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return;
        await _userManager.RemoveFromRoleAsync(user, role);
    }
    public async Task<List<string>> GetUserRolesAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return [];
        return (await _userManager.GetRolesAsync(user)).ToList();
    }
    public async Task<bool> IsUserInRoleAsync(string userName, string role)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return false;
        return await _userManager.IsInRoleAsync(user, role);
    }
    public async Task<List<ApplicationUser>> GetUsersInRoleAsync(string role)
    {
        return (await _userManager.GetUsersInRoleAsync(role)).ToList();
    }
}
