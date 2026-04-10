namespace GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.ValueObjects;

public sealed record PerformanceInfoResponse
{
    public string Uptime { get; init; } = "0d, 0h, 0m";
    public string AvgLoad { get; init; } = "0.00, 0.00, 0.00";
    public string PhysicalMem { get; init; } = "0G 0G 0G";
    public string SwapMem { get; init; } = "0M 0M 0M";
    public string DiskAvailable { get; init; } = "0G";
    public string DiskServerFiles { get; init; } = "0G";
    public string DiskBackups { get; init; } = "0G";
}
