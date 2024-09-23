using API.Models;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        var responseModel = new ApiResponseModel<object>
        {
            Status = "error",
            Message = "An internal server error occurred.",
            Errors = new Dictionary<string, string[]> { { "Exception", new[] { exception.Message } } },
            StatusCode = (int)statusCode
        };

        var result = JsonSerializer.Serialize(responseModel);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(result);
    }
}
