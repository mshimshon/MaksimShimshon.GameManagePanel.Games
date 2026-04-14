namespace GameHost.Games.Lib.LinuxGameServerManager;

internal interface IServerInstallService
{
    Task DownloadSoftwareAsync(string serverName, CancellationToken ct = default);
    Task UpdateSoftwareAsync(string serverName, CancellationToken ct = default);
    Task InstallGameServerAsync(string serverName, CancellationToken ct = default);
    Task ValidateGameServerAsync(string serverName, CancellationToken ct = default);
    Task UpdateGameServerAsync(string serverName, CancellationToken ct = default);
    Task<IEnumerable<string>> CheckUpdateGameServerAsync(string serverName, CancellationToken ct = default);
}
