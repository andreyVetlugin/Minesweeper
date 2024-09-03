using System.Text.Json.Serialization;

namespace Minesweeper.ApplicationLayer.Views;

public record NewGameRequest(
    int Width,
    int Height,
    [property: JsonPropertyName("mines_count")]
    int MinesCount);