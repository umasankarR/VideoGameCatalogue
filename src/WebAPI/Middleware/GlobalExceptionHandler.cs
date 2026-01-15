using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace WebAPI.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

        var (statusCode, response) = GetExceptionDetails(exception);

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private static (HttpStatusCode StatusCode, object Response) GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            NotFoundException notFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = notFoundException.Message,
                    Type = "NotFound"
                }),

            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                new ValidationErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = validationException.Message,
                    Type = "ValidationError",
                    Errors = validationException.Errors
                }),

            BadRequestException badRequestException => (
                HttpStatusCode.BadRequest,
                new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = badRequestException.Message,
                    Type = "BadRequest"
                }),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized access.",
                    Type = "Unauthorized"
                }),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later.",
                    Type = "InternalServerError"
                })
        };
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ValidationErrorResponse : ErrorResponse
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}
