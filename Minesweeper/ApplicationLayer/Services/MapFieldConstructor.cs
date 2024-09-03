using Minesweeper.ApplicationLayer.Models;

namespace Minesweeper.ApplicationLayer.Services;

public class MapFieldConstructor(Map map)
{
    public void FillMapWithUnknownField()
    {
        map.Field = GetUnknownField();
    }

    public void FillMapWithMines(ref readonly Point turnCoordinate)
    {
        map.MinesCoordinates = GenerateMinesCoordinates(turnCoordinate);
    }

    public void RevealNeighborCells(ref readonly Point startPosition)
    {
        ThrowIfFieldNull();

        var currentCheckingCellsCoordinates = new Stack<Point>();
        currentCheckingCellsCoordinates.Push(startPosition);
        var visitedCellsCoordinates = new HashSet<Point> { startPosition };

        while (currentCheckingCellsCoordinates.Count != 0)
        {
            var currentCellCoordinate = currentCheckingCellsCoordinates.Pop();
            var neighborsMinesCount = GetCellNeighborsMinesCount(in currentCellCoordinate);
            if (neighborsMinesCount == 0)
            {
                foreach (var neighborsCoordinate in GetNeighborsCoordinates(in currentCellCoordinate))
                {
                    if (!visitedCellsCoordinates.Contains(neighborsCoordinate) &&
                        IsCellForReveal(in neighborsCoordinate))
                    {
                        visitedCellsCoordinates.Add(neighborsCoordinate);
                        currentCheckingCellsCoordinates.Push(neighborsCoordinate);
                    }
                }
            }
            map.Field![currentCellCoordinate.Y][currentCellCoordinate.X] = neighborsMinesCount.ToString()[0];
            map.RevealedCellsCount++;
        }
    }

    public void RevealFullMap(char symbolForMines)
    {
        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                var coordinate = new Point(x, y);
                if (IsCellWithMine(in coordinate))
                    map.Field![coordinate.Y][coordinate.X] = symbolForMines;
                else
                    map.Field![coordinate.Y][coordinate.X] = GetCellNeighborsMinesCount(in coordinate).ToString()[0];
            }
        }
    }

    //TODO: сделать удаление игры по таймеру?
    private List<Point> GenerateMinesCoordinates(Point firstTurnCoordinate)
    {
        var potentialMinesCoordinates =
            GetCoordinatesCartesianProduct(map.Width, map.Height)
                .Where(p => p != firstTurnCoordinate)
                .ToList();

        var minesCoordinates = new List<Point>(map.MinesCount);

        for (var i = 0; i < map.MinesCount; i++)
        {
            var random = new Random();
            var indexOfMineCoordinate = random.Next(0, potentialMinesCoordinates.Count);
            var mineCoordinate = potentialMinesCoordinates[indexOfMineCoordinate];
            potentialMinesCoordinates.RemoveAt(indexOfMineCoordinate);
            minesCoordinates.Add(mineCoordinate);
        }

        return minesCoordinates;
    }

    private List<List<char>> GetUnknownField()
    {
        var (width, height) = (map.Width, map.Height);
        var field = new List<List<char>>(height);
        for (var y = 0; y < height; y++)
        {
            field.Add(new List<char>(width));
            for (var x = 0; x < width; x++)
            {
                field[y].Add(Constants.UnknownCell);
            }
        }

        return field;
    }

    private List<Point> GetCoordinatesCartesianProduct(int maxX, int maxY)
        => Enumerable.Range(0, maxX)
            .SelectMany(x => Enumerable.Range(0, maxY),
                (x, y) => new Point(x, y)).ToList();

    private int GetCellNeighborsMinesCount(ref readonly Point cellCoordinate)
    {
        ThrowIfMinesCoordinatesNull();

        return GetNeighborsCoordinates(in cellCoordinate)
            .Count(neighborCoordinate => IsCellWithMine(ref neighborCoordinate));
    }

    private bool IsCellForReveal(ref readonly Point cellCoordinate) =>
        !IsCellWithMine(in cellCoordinate)
        && !IsVisitedCellInPreviousTurns(in cellCoordinate);

    private bool IsVisitedCellInPreviousTurns(ref readonly Point cellsCoordinate)
        => map.Field![cellsCoordinate.Y][cellsCoordinate.X] != Constants.UnknownCell;


    private IEnumerable<Point> GetNeighborsCoordinates(ref readonly Point cellCoordinate)
    {
        var neighborsCoords = new Point[]
        {
            cellCoordinate with { X = cellCoordinate.X - 1 }, cellCoordinate with { X = cellCoordinate.X + 1 },
            cellCoordinate with { Y = cellCoordinate.Y - 1 }, cellCoordinate with { Y = cellCoordinate.Y + 1 },
            new Point(X: cellCoordinate.X - 1, Y: cellCoordinate.Y - 1),
            new Point(X: cellCoordinate.X - 1, Y: cellCoordinate.Y + 1),
            new Point(X: cellCoordinate.X + 1, Y: cellCoordinate.Y - 1),
            new Point(X: cellCoordinate.X + 1, Y: cellCoordinate.Y + 1),
        };
        return neighborsCoords.Where(IsCoordinateInMapBound);
    }

    private bool IsCoordinateInMapBound(Point cellCoordinate)
        => cellCoordinate.X < map.Width
           && cellCoordinate.X >= 0
           && cellCoordinate.Y < map.Height
           && cellCoordinate.Y >= 0;

    private bool IsCellWithMine(ref readonly Point cellCoordinate)
        => map.MinesCoordinates!.Contains(cellCoordinate);

    private void ThrowIfFieldNull()
    {
        if (map.Field == null)
            throw new NullReferenceException("Обращение к несозданному полю");
    }

    private void ThrowIfMinesCoordinatesNull()
    {
        if (map.Field == null)
            throw new NullReferenceException("Расположение мин не установлено");
    }
}