using Microsoft.AspNetCore.Mvc;

namespace Project.Application.Controllers;

[ApiController]
[Authorize]
public class BaseController : Controller
{
}
