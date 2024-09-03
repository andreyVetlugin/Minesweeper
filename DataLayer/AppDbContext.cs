using System.Text.Json;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;


namespace DataLayer;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Map> Maps { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasOne(x => x.Map);
        modelBuilder.Entity<Map>().Property(x => x.MinesCoordinates).HasColumnType("point[]").HasConversion(
            v => v!.Select(p => new NpgsqlPoint(p.X, p.Y)).ToArray(), // Конвертация C# List<Point> в массив NpgsqlPoint
            v => v.Select(p => new Point((int)p.X, (int)p.Y)).ToList() // Конвертация массива NpgsqlPoint в C# List<Point>
        );
        modelBuilder.Entity<Map>().Property(x => x.Field).HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // Преобразование из массива в JSON строку при сохранении
            v => JsonSerializer.Deserialize<List<List<char>>>(v, (JsonSerializerOptions)null));
    }
}