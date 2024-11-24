
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CommonBoilerPlateEight.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string status = Notify.Error.ToString();
        if (exception is BaseException customException)
        {
            statusCode = (int)customException.StatusCode;
            status = Notify.Info.ToString();
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
        }
        var response = new ApiResponseModel
        {
            StatusCode = statusCode,
            Message = "An error occurred while processing your request",
            Errors = new List<string> { exception.Message },
            Status = status
        };

        var result = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        return context.Response.WriteAsync(result);
    }
}