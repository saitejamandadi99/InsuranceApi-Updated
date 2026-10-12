using InsuranceApi.DTO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
namespace InsuranceApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var (statusCode, errorType, message) = ex switch
                {
                    DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Concurrency Conflict", "This record is being modified by another user. Please refresh and try again"),
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found", ex.Message),
                    UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", ex.Message),
                    _ => (StatusCodes.Status400BadRequest, "Bad Request", "An unexpected error occurred. Please try again later.")
                };
                context.Response.StatusCode = statusCode;

                var response = new ErrorResponseDTO
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorType = errorType,
                    Message = ex.Message,
                    RequestPath = context.Request.Path
                };

                _logger.LogError(ex,"Unhandled exception occurred while processing {Path}",context.Request.Path);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
