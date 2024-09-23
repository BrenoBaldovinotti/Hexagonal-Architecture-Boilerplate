using API.Models;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class ValidationErrorMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        var validationFailures = exception.Errors;
        var errors = validationFailures
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        var responseModel = new ApiResponseModel<object>
        {
            Status = "error",
            Message = "Validation failed",
            Errors = errors,
            StatusCode = (int)HttpStatusCode.BadRequest
        };

        var result = JsonSerializer.Serialize(responseModel);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return context.Response.WriteAsync(result);
    }
}
