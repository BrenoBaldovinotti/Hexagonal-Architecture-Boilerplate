using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v{version:apiVersion}/admin")]
public class AdminController(IAuthorizationService authorizationService) : BaseController(authorizationService)
{
    [HttpGet("dashboard")]
    public IActionResult GetAdminDashboard()
    {
        return Success("This is the admin dashboard, accessible only by Admin users.");
    }
}