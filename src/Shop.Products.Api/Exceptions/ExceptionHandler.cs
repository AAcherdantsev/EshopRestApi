using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Shop.Products.Api.Exceptions;

/// <summary>
/// Provides a mechanism to handle exceptions globally in an ASP.NET Core application.
/// </summary>
public static class ExceptionHandler
{
    /// <summary>
    /// Configures a custom global exception handler for the application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure the exception handler for.</param>
    public static void UseCustomExceptionHandler(this WebApplication app)
    {
        var isDev = app.Environment.IsDevelopment();

        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature == null) return;

                var ex = exceptionHandlerFeature.Error;

                var result = isDev
                    ? JsonSerializer.Serialize(new
                    {
                        message = ex.Message,
                        stackTrace = ex.StackTrace
                    })
                    : JsonSerializer.Serialize(new
                    {
                        message = ex.Message
                    });

                await context.Response.WriteAsync(result);
            });
        });
    }
}