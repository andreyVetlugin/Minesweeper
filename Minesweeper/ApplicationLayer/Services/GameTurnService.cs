using Minesweeper.ApplicationLayer.CustomExceptions;
using Minesweeper.ApplicationLayer.Models;

namespace Minesweeper.ApplicationLayer.Services;

public class GameTurnService
{
    public void MakeTurn(Game game, Point turnCoordinate)
    {
        var fieldConstructor = new MapFieldConstructor(game.Map);

        if (game.IsEnded)
            throw new InvalidTurnParameterException("игра завершена");

        if (!IsCoordinateInGameMapBound(game, turnCoordinate))
            throw new InvalidTurnParameterException("координаты хода выходят за границу карты");

        if (IsCellRevealAlready(game, in turnCoordinate))
            throw new InvalidTurnParameterException("уже открытая ячейка");

        if (IsFirstTurnInGame(game))
            fieldConstructor.FillMapWithMines(in turnCoordinate);

        if (IsLosingTurn(game, turnCoordinate))
        {
            fieldConstructor.RevealFullMap(Constants.BombCellOnLosing);
            game.IsEnded = true;
        }
        else
        {
            fieldConstructor.RevealNeighborCells(in turnCoordinate);
        }

        if (IsGameWon(game))
        {
            fieldConstructor.RevealFullMap(Constants.BombCellOnWining);
            game.IsEnded = true;
        }
    }

    private static bool IsCellRevealAlready(Game game, in Point turnCoordinate)
        => game.Map.Field![turnCoordinate.Y][turnCoordinate.X] != Constants.UnknownCell;

    private static bool IsFirstTurnInGame(Game game)
        => game.Map.MinesCoordinates == null;

    private static bool IsLosingTurn(Game game, in Point turnCoordinate)
        => game.Map.MinesCoordinates!.Contains(turnCoordinate);

    private static bool IsGameWon(Game game)
        => game.Map.RevealedCellsCount + game.Map.MinesCount == game.Map.Width * game.Map.Height;

    private static bool IsCoordinateInGameMapBound(Game game, Point cellCoordinate)
        => cellCoordinate.X < game.Map.Width
           && cellCoordinate.X >= 0
           && cellCoordinate.Y < game.Map.Height
           && cellCoordinate.Y >= 0;
}