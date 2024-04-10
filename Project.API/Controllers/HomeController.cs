using Project.Auth.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Application.Controllers;

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

    [HttpGet(Name = "GetConfig")]
    [ProducesResponseType(typeof(IEnumerable<KeyValuePair<string, string>>), StatusCodes.Status200OK)]
    [Authorize(Roles = UserRoles.Developer)]
    public ActionResult GetConfig([FromServices] IConfiguration config)
    {
        return Ok(config.AsEnumerable());
    }
}
