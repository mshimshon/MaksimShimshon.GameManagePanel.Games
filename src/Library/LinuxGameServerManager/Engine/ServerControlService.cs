using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response;
using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.Enums;
using GameHost.Games.Lib.LinuxGameServerManager.Exceptions;
using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;

namespace GameHost.Games.Lib.LinuxGameServerManager.Engine;

internal class ServerControlService : IServerControlService
{
    private readonly ILinuxCommand _linuxCommand;

    public ServerControlService(ILinuxCommand linuxCommand)
    {
        _linuxCommand = linuxCommand;
    }
    public Task ConsoleAsync(string serverName, Func<string, Task> consoleStream, CancellationToken ct = default) => throw new NotImplementedException();

    public async Task<IReadOnlyCollection<DetailsResponse>> DetailsAsync(string serverName, bool hideSensitive = false, CancellationToken ct = default)
    {
        string cmd = hideSensitive ? "postdetails" : "details";
        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" {cmd}")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new DetailsServerFailedException(result.StandardError);
        var outputList = new List<DetailsResponse>();
        DetailsResponse? lastEntry = default;
        bool capture = false;
        foreach (var item in result.RawStandardOutput)
        {
            if (item.Contains("======") && !capture)
            {
                capture = true;
                continue;
            }
            else if (item.Contains("======") && !capture)
            {
                if (lastEntry != default)
                    outputList.Remove(lastEntry);
                continue;
            }
            bool port =
                (item.Contains("INBOUND", StringComparison.OrdinalIgnoreCase) ||
                item.Contains("OUTBOUND", StringComparison.OrdinalIgnoreCase)) && (item.Contains("tcp", StringComparison.OrdinalIgnoreCase) ||
                item.Contains("udp", StringComparison.OrdinalIgnoreCase));

            lastEntry = new(item, port ? DetailType.Port : DetailType.None);
            outputList.Add(lastEntry);
        }

        return outputList.AsReadOnly();
    }

    public async Task RestartAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" restart")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new RestartServerFailedException(result.StandardError);
    }
    public async Task StartAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" start")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new StartServerFailedException(result.StandardError);
    }
    public async Task StopAsync(string serverName, CancellationToken ct = default)
    {
        var result = await _linuxCommand
            .BuildCommand($"cd \"{BaseInfo.HOME}\"")
            .AndCommand($"./\"{serverName}\" stop")
            .AsUser(BaseInfo.USERNAME)
            .ExecAsync(ct);
        if (result.Failed)
            throw new StopServerFailedException(result.StandardError);
    }
}
