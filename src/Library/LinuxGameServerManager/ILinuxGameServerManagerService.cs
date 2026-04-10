namespace GameHost.Games.Lib.LinuxGameServerManager;

public interface ILinuxGameServerManagerService
{
    Task InstallAsync(string serverName, CancellationToken ct = default);
    Task BackupAsync(CancellationToken ct = default);
    Task GetBackupsAsync(CancellationToken ct = default);
    Task RestoreBackupAsync(string name, CancellationToken ct = default);
    Task ConsoleAsync(Func<string, Task> consoleStream, CancellationToken ct = default);
    Task StopAsync(CancellationToken ct = default);
    Task StartAsync(CancellationToken ct = default);
    Task RestartAsync(CancellationToken ct = default);
    Task ValidateAsync(CancellationToken ct = default);
    Task DetailsAsync(bool hideSensitive = false, CancellationToken ct = default);
    Task UpdateGameAsync(CancellationToken ct = default);
    Task UpdateSoftwareAsync(CancellationToken ct = default);
}
