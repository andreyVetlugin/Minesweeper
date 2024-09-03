namespace DataLayer.Models;

public class Map
{
    public Guid Id { get; set; }
    public required List<List<char>> Field { get; set; }
    public List<Point>? MinesCoordinates { get; set; }
    public bool IsMapInit { get; set; } = false;
    public int Width { get; set; }
    public int Height { get; set; }
    public int MinesCount { get; set; }
    public int RevealedCellsCount { get; set; }
}

public record struct Point(int X, int Y);