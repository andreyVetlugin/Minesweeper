using DataLayer;
using Minesweeper.ApplicationLayer.Middleware;

namespace Minesweeper.ApplicationLayer.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        return app;
    }

    public static WebApplication CreateDbInNeeded(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();

        return app;
    }

    public static WebApplication UseSwaggerInDefMode(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix =
                    string.Empty;
            });
        }

        return app;
    }
}