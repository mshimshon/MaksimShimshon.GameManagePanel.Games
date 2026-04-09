namespace GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

public sealed record ConnectionInfoResponse
{

    public ConnectionInfoResponse(string address, ICollection<PortInfoResponse> portInfoResponses)
    {
        Address = address;
        PortInfoResponses = portInfoResponses;
    }

    public string Address { get; }
    public ICollection<PortInfoResponse> PortInfoResponses { get; }
}
