using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController(IAuthorizationService authorizationService) : ControllerBase
{
    protected IActionResult Success<T>(T data, string message = "Request processed successfully.")
    {
        return Ok(new ApiResponseModel<T>
        {
            Status = "success",
            Data = data,
            Message = message,
            StatusCode = StatusCodes.Status200OK
        });
    }

    protected IActionResult Success(string message = "Request processed successfully.")
    {
        return Ok(new ApiResponseModel<object>
        {
            Status = "success",
            Message = message,
            StatusCode = StatusCodes.Status200OK
        });
    }

    protected IActionResult Error(string message, Dictionary<string, string[]>? errors = null)
    {
        return BadRequest(new ApiResponseModel<object>
        {
            Status = "error",
            Message = message,
            Errors = errors,
            StatusCode = StatusCodes.Status400BadRequest
        });
    }

    protected async Task<IActionResult> AuthorizeAndExecuteAsync(object resource, string[] policyNames, Func<Task<IActionResult>> action)
    {
        foreach (var policyName in policyNames)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, resource, policyName);

            if (authResult.Succeeded)
            {
                return await action();
            }

            var statusCode = authResult.Failure.FailedRequirements.OfType<IAuthorizationRequirement>().Any()
                ? StatusCodes.Status403Forbidden
                : StatusCodes.Status401Unauthorized;

            return StatusCode(statusCode, new
            {
                error = new
                {
                    code = statusCode,
                    message = statusCode == StatusCodes.Status403Forbidden
                        ? "You do not have permission to access this resource."
                        : "Authentication credentials are missing or invalid.",
                    details = "Ensure that you have the appropriate permissions and that your token is valid."
                }
            });
        }

        return StatusCode(StatusCodes.Status403Forbidden, new { message = "Authorization failed for all policies" });
    }
}
