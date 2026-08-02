using System.Text.Json;
using MesaSitec.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace MesaSitec.API.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";

        if (exception is BusinessException businessException)
        {
            httpContext.Response.StatusCode =
                businessException.StatusCode;

            var response = new
            {
                type = $"https://mesasitec.local/errors/{businessException.Codigo.ToLower().Replace('_', '-')}",
                title = businessException.Title,
                status = businessException.StatusCode,
                detail = businessException.Message,
                codigo = businessException.Codigo,
                errors = businessException.Errors
            };

            await httpContext.Response.WriteAsync(
                JsonSerializer.Serialize(response),
                cancellationToken);

            return true;
        }

        httpContext.Response.StatusCode = 500;

        var internalError = new
        {
            type = "https://mesasitec.local/errors/internal-server-error",
            title = "Error interno",
            status = 500,
            detail = "Ocurrió un error inesperado.",
            codigo = "ERROR_INTERNO"
        };

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(internalError),
            cancellationToken);

        return true;
    }
}