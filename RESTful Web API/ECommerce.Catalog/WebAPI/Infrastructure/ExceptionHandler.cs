using ECommerce.Catalog.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace ECommerce.Catalog.WebAPI.Infrastructure;

public static class ExceptionHandler
{
    public static void HandleExceptions(this WebApplication app)
    {
        app.UseExceptionHandler(err => err.Run(async context =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            (int status, string message) = exception switch
            {
                NotFoundException ex => (404, ex.Message),
                ConflictException ex => (409, ex.Message),
                _ => (500, "An unexpected error occurred.")
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new { error = message });
        }));
    }
}
