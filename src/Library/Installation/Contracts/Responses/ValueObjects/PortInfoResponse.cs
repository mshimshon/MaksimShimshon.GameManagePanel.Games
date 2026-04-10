namespace GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;


public sealed record PortInfoResponse
{
    public PortInfoResponse(string name, string port, string protocol)
    {
        Name = name;
        Port = port;
        Protocol = protocol;
    }

    public string Name { get; }
    public string Port { get; }
    public string Protocol { get; }
}
