using Demo.Domain.Exceptions;

namespace Demo.Presentation.ExceptionHandling;

public sealed class ValidationExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                title = "Validation failed",
                errors = exception.Errors
            });
        }
    }
}
