namespace DataLayer.Models;

public class Game
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public required Map Map { get; set; }
    public bool IsEnded { get; set; } = false;
}