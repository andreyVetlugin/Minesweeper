using Minesweeper.ApplicationLayer.Extensions;

namespace Minesweeper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args).AddSwagger().AddDefaultCorsPolicy().RegisterServices();

        var app = builder.Build().UseMiddlewares().UseSwaggerInDefMode().CreateDbInNeeded();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}