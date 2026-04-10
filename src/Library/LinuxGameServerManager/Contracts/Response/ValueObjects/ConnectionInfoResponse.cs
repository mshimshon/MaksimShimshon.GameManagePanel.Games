namespace GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.ValueObjects;

public sealed record ConnectionInfoResponse
{

    public ConnectionInfoResponse(string name, string address, ICollection<PortInfoResponse> portInfoResponses)
    {
        Name = name;
        Address = address;
        PortInfoResponses = portInfoResponses;
    }
    public string? Password { get; init; }
    public string Name { get; }
    public string Address { get; }
    public ICollection<PortInfoResponse> PortInfoResponses { get; }
}
