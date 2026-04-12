using GameHost.Games.Lib.LinuxGameServerManager.Exceptions;
using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;

namespace GameHost.Games.Lib.LinuxGameServerManager.Engine;

internal class LinuxGameServerManagerService : ILinuxGameServerManagerService
{
    private readonly ILinuxCommand _linuxCommand;
    private const string USERNAME = "lgsm";
    private const string HOME = "/home/lgsm/";
    private readonly string[] _dependencies = ["curl", "jq"];
    public LinuxGameServerManagerService(ILinuxCommand linuxCommand)
    {
        _linuxCommand = linuxCommand;
    }
    public Task BackupAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task ConsoleAsync(Func<string, Task> consoleStream, CancellationToken ct = default) => throw new NotImplementedException();
    public Task DetailsAsync(bool hideSensitive = false, CancellationToken ct = default) => throw new NotImplementedException();
    public Task GetBackupsAsync(CancellationToken ct = default) => throw new NotImplementedException();
    /*
     runuser -l "$LGSM_USER" -c "
    set -euo pipefail
    set -E
    export DEBIAN_FRONTEND=noninteractive
    cd \"$LGSM_HOME\"
    curl -Lo linuxgsm.sh https://linuxgsm.sh
    chmod +x linuxgsm.sh
    bash ./linuxgsm.sh \"$GAME_SERVER\"
    bash \"./$GAME_SERVER\" auto-install
" >&2
     */
    private async Task CheckAndInstallDependenciesAsync(CancellationToken ct = default)
    {
        // check if curl is installed
        string dependencies = string.Join(' ', _dependencies);
        bool depInstalled = await _linuxCommand
            .BuildCommand($"dpkg -s {dependencies} >/dev/null")
            .AndPrintPayload("true")
            .OrPrintPayload("false")
            .Sudo()
            .ExecOrDefaultAsync(false, ct);
        if (ct.IsCancellationRequested) return;
        if (!depInstalled)
        {
            int maxTry = 2;
            do
            {
                if (ct.IsCancellationRequested) return;
                bool success = await _linuxCommand
                .BuildCommand($"apt-get install -y {dependencies}")
                .AndPrintPayload("true")
                .OrPrintPayload("false")
                .Sudo()
                .ExecOrDefaultAsync(false, ct);
                if (ct.IsCancellationRequested) return;
                if (!success && maxTry > 0)
                {
                    maxTry--;
                    continue;
                }
                else if (success) break;
                else
                {
                    throw new DependencyInstallFailedException(_dependencies);
                }

            } while (maxTry >= 0 || ct.IsCancellationRequested);

        }
    }
    private async Task DownloadLinuxGameServerManagerAsync(CancellationToken ct = default)
    {
        int maxTry = 2;
        do
        {
            if (ct.IsCancellationRequested) return;
            var downloadLgsm = await _linuxCommand
                .BuildCommand($"cd \"{HOME}\"")
                .AndCommand("curl -Lo linuxgsm.sh https://linuxgsm.sh")
                .AndCommand("chmod +x linuxgsm.sh")
                .Sudo()
                .ExecAsync();
            if (ct.IsCancellationRequested) return;
            if (downloadLgsm.Failed && maxTry > 0)
            {
                maxTry--;
                continue;
            }
            else if (!downloadLgsm.Failed) break;
            else
            {
                throw new LinuxGameServerManagerDownloadHasFailedException(downloadLgsm);
            }

        } while (maxTry >= 0 || ct.IsCancellationRequested);
    }

    public async Task InstallAsync(string serverName, CancellationToken ct = default)
    {
        try
        {
            // TODO: Acquire Lock and Grant Sudo Permission to LGSM
            await CheckAndInstallDependenciesAsync(ct);
            await DownloadLinuxGameServerManagerAsync(ct);
        }
        catch
        {
            throw;
        }
        finally
        {
            // TODO: Release Lock and Release Sudo Passwordless Permissions
        }
    }
    public Task RestartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task RestoreBackupAsync(string name, CancellationToken ct = default) => throw new NotImplementedException();
    public Task StartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task StopAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task UpdateGameAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task UpdateSoftwareAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task ValidateAsync(CancellationToken ct = default) => throw new NotImplementedException();
}
