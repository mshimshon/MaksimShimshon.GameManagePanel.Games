using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

namespace GameHost.Games.Lib.Installation.Contracts.Responses;

public sealed record ResultResponse
{
    public object? Data { get; init; }
    public ErrorResponse? Error { get; init; }
}
