namespace GameHost.Games.Lib.Installation.Contracts.Responses;

public sealed class GameStartupParameterAllowedValueResponse
{
    public string Value { get; init; } = default!;
    public string Label { get; init; } = default!;
}
