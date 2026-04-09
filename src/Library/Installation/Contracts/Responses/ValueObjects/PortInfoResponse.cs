namespace GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;


public sealed record PortInfoResponse
{
    public PortInfoResponse(string name, string port)
    {
        Name = name;
        Port = port;
    }

    public string Name { get; }
    public string Port { get; }

}
