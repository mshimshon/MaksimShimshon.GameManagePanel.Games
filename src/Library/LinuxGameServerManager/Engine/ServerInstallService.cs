using GameHost.Games.Lib.LinuxGameServerManager.Exceptions;
using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;

namespace GameHost.Games.Lib.LinuxGameServerManager.Engine;

internal class ServerInstallService : IServerInstallService
{
    private readonly ILinuxCommand _linuxCommand;
    public ServerInstallService(ILinuxCommand linuxCommand)
    {
        _linuxCommand = linuxCommand;
    }
    public async Task InstallGameServerAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
                .BuildCommand($"cd \"{BaseInfo.HOME}\"")
                .AndCommand($"/bib/bash ./linuxgsm.sh \"{serverName}\"")
                .AndCommand($"/bib/bash \"./{serverName}\" auto-install")
                .AsUser(BaseInfo.USERNAME)
                .ExecAsync(ct);
        if (result.Failed) throw new DownloadHasFailedException(result.StandardError, $"Couldn't install the game server for {serverName}");

    }
    public async Task UpdateGameServerAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" force-update")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new UpdateGameFailedException(serverName);
    }
    public async Task ValidateGameServerAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
                        .BuildCommand($"cd \"{BaseInfo.HOME}\"")
                        .AndCommand($"./\"{serverName}\" validate")
                        .AsUser(BaseInfo.USERNAME)
                        .ExecAsync(ct);
        if (result.Failed)
            throw new GameValidationFailedException(serverName);
    }

    private async Task InstallSoftwareDependenciesAsync(CancellationToken ct = default)
    {
        string depInline = string.Join(' ', BaseInfo.Dependencies);
        var result = await _linuxCommand
           .BuildCommand($"apt install -y {depInline}")
           .ExecAsync(ct);
        if (result.Failed)
            throw new DownloadHasFailedException(result.StandardError, $"Couldn't install the one or more dependencies {depInline}.");
    }

    public async Task DownloadSoftwareAsync(string serverName, CancellationToken ct = default)
    {
        await InstallSoftwareDependenciesAsync();
        var result = await _linuxCommand
                .BuildCommand($"cd \"{BaseInfo.HOME}\"")
                .AndCommand("curl -Lo linuxgsm.sh https://linuxgsm.sh")
                .AndCommand("chmod +x linuxgsm.sh")
                .ExecAsync(ct);
        if (result.Failed) throw new DownloadHasFailedException(result.StandardError, $"Couldn't install the LGSM scripts.");
    }

    public async Task UpdateSoftwareAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" update-lgsm")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new UpdateSoftwareFailedException(result.StandardError);
    }

    public async Task<IEnumerable<string>> CheckUpdateGameServerAsync(string serverName, CancellationToken ct = default)
    {

        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" check-update")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new CheckUpdateFailedException(result.StandardOutput, result.StandardError, serverName);
        return result.RawStandardOutput;
    }
}
