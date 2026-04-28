using GameHost.Games.Lib.Installation;
using GameHost.Games.Lib.Installation.Contracts.Responses.Status;
using GameHost.Games.Lib.LinuxGameServerManager;
using LinuxGameServerManager.Extension;

namespace GameHost.Games.ProjectZomboid.Console.Services;

internal class ServerControlService : IServerControl
{
    private readonly ILinuxGameServerManagerService _linuxGameServerManagerService;

    public ServerControlService(ILinuxGameServerManagerService linuxGameServerManagerService)
    {
        _linuxGameServerManagerService = linuxGameServerManagerService;
    }
    public async Task ConsoleAsync(Func<string, Task> consoleStream, CancellationToken ct = default)
        => await _linuxGameServerManagerService.ConsoleAsync(BaseInfo.LGSM_SERVER_ID, consoleStream, ct);

    public async Task RestartAsync(CancellationToken ct = default)
        => await _linuxGameServerManagerService.RestartAsync(BaseInfo.LGSM_SERVER_ID, ct);
    public async Task StartAsync(CancellationToken ct = default)
        => await _linuxGameServerManagerService.StartAsync(BaseInfo.LGSM_SERVER_ID, ct);

    public async Task<ServerStatusResponse> StatusAsync(CancellationToken ct = default)
    {
        var details = await _linuxGameServerManagerService.DetailsAsync(BaseInfo.LGSM_SERVER_ID, false, ct);
        return details.ConvertToServerStatus();
    }
    public async Task StopAsync(CancellationToken ct = default)
        => await _linuxGameServerManagerService.StopAsync(BaseInfo.LGSM_SERVER_ID, ct);

}
