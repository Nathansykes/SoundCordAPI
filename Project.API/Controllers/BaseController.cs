using Microsoft.AspNetCore.Mvc;

namespace Project.API.Controllers;

[ApiController]
[Authorize]
public class BaseController : Controller
{
}
