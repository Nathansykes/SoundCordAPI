using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Auth.Roles;
using Project.Domain;

namespace Project.API.Controllers;

public class HomeController() : BaseController
{
    

    [HttpGet]
    [Route("/")]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return Ok("Hello World");
    }

    
}

public record UserInfoModel(string UserName, IEnumerable<string> Roles);