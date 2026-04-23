using GameHost.Games.Lib.Installation.Contracts.Responses.Enums;
using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

namespace GameHost.Games.Lib.Installation.Contracts.Responses;

public sealed record ServerStatusResponse
{
    public ServerStatus Status { get; set; } = ServerStatus.Unknown;
    public ConnectionInfoResponse? ConnectionInfo { get; set; }
}
