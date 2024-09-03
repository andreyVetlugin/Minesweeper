using Microsoft.AspNetCore.Mvc;
using Minesweeper.ApplicationLayer.Models;
using Minesweeper.ApplicationLayer.Services;
using Minesweeper.ApplicationLayer.Views;

namespace Minesweeper.ApplicationLayer.Controllers;

public class GameController(GameService gameService) : Controller
{
    [HttpPost("/turn")]
    public async Task<GameInfoResponse> Turn([FromBody] GameTurnRequest gameTurnRequest)
    {
        return await gameService.MakeTurn(gameTurnRequest.GameId, new Point(gameTurnRequest.Col, gameTurnRequest.Row));
    }

    [HttpPost("/new")]
    public async Task<GameInfoResponse> Create([FromBody] NewGameRequest newGameRequest)
    {
        return await gameService.CreateGame(newGameRequest.Width, newGameRequest.Height, newGameRequest.MinesCount);
    }
}