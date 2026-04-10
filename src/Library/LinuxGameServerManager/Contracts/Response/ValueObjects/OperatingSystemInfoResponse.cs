namespace GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.ValueObjects;

public sealed record OperatingSystemInfoResponse
{
    public OperatingSystemInfoResponse(string distro, string architecture, string kernel)
    {
        Distro = distro;
        Architecture = architecture;
        Kernel = kernel;
    }
    public string? Hostname { get; init; }
    public string? RemoteConsolePassword { get; init; }
    public string? TMUX { get; init; }
    public string? GLIBC { get; init; }
    public string Distro { get; }
    public string Architecture { get; }
    public string Kernel { get; }
}
