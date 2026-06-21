using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security;
using System.Text.Json;

internal class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid().ToString();

            _logger.LogError(ex, "Error Occurred | CorrelationId: {CorrelationId}", correlationId);

            var statusCode = ex switch
            {
                KeyNotFoundException => 404,
                ArgumentException => 400,
                UnauthorizedAccessException => 401,
                SecurityException => 403,
                DbUpdateException => 400,
                SqlException => 503,
                _ => 500
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            object response = _env.IsDevelopment()
                ? new
                {
                    statusCode,
                    message = ex.Message,
                    trace = ex.StackTrace,
                    correlationId
                }
                : new
                {
                    statusCode,
                    message = "Internal Server Error",
                    correlationId
                };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await httpContext.Response.WriteAsync(json);
        }
    }
}