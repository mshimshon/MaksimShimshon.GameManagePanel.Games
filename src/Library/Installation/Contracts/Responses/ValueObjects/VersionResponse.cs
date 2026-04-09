namespace GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

public sealed record VersionResponse
{
    public VersionResponse(string version)
    {
        Version = version;
    }

    public string Version { get; }
}
