using GameHost.Games.Lib.Installation.Contracts.Responses.Enums;
using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

namespace GameHost.Games.Lib.Installation.Contracts.Responses;

public sealed record ServerStatusResponse
{
    public ServerStatus Status { get; init; } = ServerStatus.Unknown;
    public ConnectionInfoResponse? ConnectionInfo { get; init; }
}
