using Project.Auth.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.API.Controllers;

[Route("api/[Action]")]
public class HomeController : BaseController
{
    [HttpGet]
    [Route("/")]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return Ok("Hello World");
    }
}
