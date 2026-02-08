using System.Net;
using FluentValidation;
using System.Text.Json;
using todofinal.Application.DTOs;
namespace todofinal.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext); // Her şey yolundaysa devam et
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex); // Hata varsa yakala
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Sunucu tarafında bir hata oluştu.";
        List<string>? validationErrors = null;

        // Eğer hata bizim FluentValidation'dan geliyorsa:
        if (exception is ValidationException validationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = "Doğrulama hatası oluştu.";
            validationErrors = validationException.Errors.Select(x => x.ErrorMessage).ToList();
        }

        context.Response.StatusCode = statusCode;

        var response = new ErrorResultDto
        {
            StatusCode = statusCode,
            Message = message,
            Errors = validationErrors
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}