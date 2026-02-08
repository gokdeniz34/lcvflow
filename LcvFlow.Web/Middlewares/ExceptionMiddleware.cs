using System.Net;
using System.Text.Json;
using Serilog;

namespace LcvFlow.Web.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Beklenmedik bir hata oluştu: {Message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Sunucu tarafında bir hata oluştu. Lütfen sistem yöneticisi ile iletişime geçin.",
            Detailed = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

