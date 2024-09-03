using System.Text.Json.Serialization;

namespace Minesweeper.ApplicationLayer.Views;

public record GameTurnRequest(
    [property: JsonPropertyName("game_id")]
    Guid GameId,
    int Col,
    int Row);