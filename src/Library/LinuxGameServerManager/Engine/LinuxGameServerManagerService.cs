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
    private async Task CheckAndInstallDependencies(CancellationToken ct = default)
    {
        // check if curl is installed
        string dependencies = string.Join(' ', _dependencies);
        bool depInstalled = await _linuxCommand
            .BuildCommand($"dpkg -s {dependencies} >/dev/null 2>&1")
            .AndCommand("echo true")
            .OrCommand("echo false")
            .Sudo()
            .ExecOrDefaultAsync(false);
        if (ct.IsCancellationRequested) return;
        if (!depInstalled)
        {
            int maxTry = 2;
            do
            {
                if (ct.IsCancellationRequested) return;
                bool success = await _linuxCommand
                .BuildCommand($"apt-get install -y {dependencies}", true)
                .AndCommand("echo true")
                .OrCommand("echo false")
                .Sudo()
                .ExecOrDefaultAsync(false);
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
    public async Task InstallAsync(string serverName, CancellationToken ct = default)
    {
        await CheckAndInstallDependencies(ct);
        await _linuxCommand.BuildCommand($"cd \"{HOME}\"")
            .AndCommand("curl -Lo linuxgsm.sh https://linuxgsm.sh")
            .Sudo().AsUser(USERNAME)
            .SetWorkingDir(HOME)
            .ExecAsync();
        await _linuxCommand.BuildCommand("").AsUser(USERNAME).Sudo().ExecAsync();
    }
    public Task RestartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task RestoreBackupAsync(string name, CancellationToken ct = default) => throw new NotImplementedException();
    public Task StartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task StopAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task UpdateGameAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task UpdateSoftwareAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task ValidateAsync(CancellationToken ct = default) => throw new NotImplementedException();
}
