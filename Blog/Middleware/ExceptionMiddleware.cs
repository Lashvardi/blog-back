using System.Net;
using Blog.Exceptions;
using Blog.Models.API_CORE;

namespace Blog.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            BadRequestException => HttpStatusCode.BadRequest,
            NotImplementException => HttpStatusCode.NotImplemented,
            AlreadyExistException => HttpStatusCode.Conflict,
            InternalServerErrorException => HttpStatusCode.InternalServerError,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            
            _ => HttpStatusCode.InternalServerError,
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            RequestId = context.TraceIdentifier,
            StackTrace = context.RequestServices.GetService<IWebHostEnvironment>().IsDevelopment() ? exception.StackTrace : null,
            Details = exception.InnerException?.Message
        };


        await context.Response.WriteAsync(response.ToString());
    }
}