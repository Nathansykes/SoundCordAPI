using Microsoft.AspNetCore.Mvc;

namespace Project.API.Controllers;
[Route("api/[controller]")]
public class SongsController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
