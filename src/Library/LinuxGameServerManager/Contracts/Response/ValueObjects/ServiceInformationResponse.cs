namespace GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.ValueObjects;

public sealed record ServiceInformationResponse
{
    public string? User { get; init; }
    public string? Location { get; init; }
    public string? ConfigFile { get; init; }
    public string ServiceName { get; }
    public string ServerStatus { get; }

    public ServiceInformationResponse(string serviceName, string serverStatus)
    {
        ServiceName = serviceName;
        ServerStatus = serverStatus;
    }
}
