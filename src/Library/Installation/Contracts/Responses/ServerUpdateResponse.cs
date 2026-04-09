using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

namespace GameHost.Games.Lib.Installation.Contracts.Responses;

public sealed record ServerUpdateResponse
{
    public VersionResponse? UpdateToVersion { get; init; }
    public VersionResponse? CurrentVersion { get; init; }
}
