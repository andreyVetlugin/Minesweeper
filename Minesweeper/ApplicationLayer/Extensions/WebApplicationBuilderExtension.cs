using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Minesweeper.ApplicationLayer.Mapping;
using Minesweeper.ApplicationLayer.Services;

namespace Minesweeper.ApplicationLayer.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
        return builder;
    }

    public static WebApplicationBuilder AddDefaultCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("https://minesweeper-test.studiotg.ru/").AllowAnyHeader().AllowAnyMethod()
                        .AllowAnyOrigin();
                });
        });
        return builder;
    }

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        
        
        builder.Services.AddTransient<GameService>();
        builder.Services.AddSingleton<GameTurnService>();
        
        builder.Services.AddSingleton<MapConstructorService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var mapSizeSection = config.GetSection("MapMaxSize");
            var maxWidth = int.Parse(mapSizeSection["maxWidth"]);
            var maxHeight = int.Parse(mapSizeSection["maxHeight"]);
            return new MapConstructorService(maxWidth, maxHeight);
        });
        return builder;
    }
}