using AutoMapper;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Minesweeper.ApplicationLayer.CustomExceptions;
using Minesweeper.ApplicationLayer.Models;
using Minesweeper.ApplicationLayer.Views;

namespace Minesweeper.ApplicationLayer.Services;

public class GameService(
    AppDbContext appContext,
    MapConstructorService mapConstructorService,
    GameTurnService gameTurnService,
    IMapper mapper)
{
    public async Task<GameInfoResponse> CreateGame(int width, int height, int minesCount)
    {
        var map = mapConstructorService.CreateMap(width, height, minesCount);
        var game = new Game
        {
            Map = map,
            IsEnded = false
        };

        var gameForSave = mapper.Map<DataLayer.Models.Game>(game);

        var gameAfterSave = await appContext.Games.AddAsync(gameForSave);
        await appContext.SaveChangesAsync();
        return mapper.Map<GameInfoResponse>(gameAfterSave.Entity);
    }

    public async Task<GameInfoResponse> MakeTurn(Guid gameId, Point turnCoordinate)
    {
        var gameFromBd = await appContext.Games.Where(x => x.Id == gameId)
            .Include(x => x.Map).FirstOrDefaultAsync();

        if (gameFromBd == null)
            throw new InvalidTurnParameterException(
                $"игра с идентификатором {gameId} не была создана или устарела (неактуальна)");
        
        var game = mapper.Map<Game>(gameFromBd);

        gameTurnService.MakeTurn(game,turnCoordinate);

        mapper.Map(game, gameFromBd);
        appContext.Entry(gameFromBd.Map).Property(x => x.Field).IsModified = true;
        await appContext.SaveChangesAsync();
        return mapper.Map<GameInfoResponse>(game);
    }
}