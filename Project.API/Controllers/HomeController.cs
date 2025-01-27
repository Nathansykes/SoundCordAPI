using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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