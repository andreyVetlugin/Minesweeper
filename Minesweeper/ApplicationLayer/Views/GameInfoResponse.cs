using System.Text.Json.Serialization;

namespace Minesweeper.ApplicationLayer.Views;

public record struct GameInfoResponse(
    [property: JsonPropertyName("game_id")]
    Guid GameId,
    int Width,
    int Height,
    [property: JsonPropertyName("mines_count")]
    int MinesCount,
    bool Completed,
    List<List<char>> Field
);