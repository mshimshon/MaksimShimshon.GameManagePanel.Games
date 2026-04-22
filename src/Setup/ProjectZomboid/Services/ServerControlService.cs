using GameHost.Games.Lib.Installation;
using GameHost.Games.Lib.Installation.Contracts.Responses;

namespace GameHost.Games.ProjectZomboid.Console.Services;

internal class ServerControlService : IServerControl
{
    public async Task ConsoleAsync(Func<string, Task> consoleStream, CancellationToken ct = default) => throw new NotImplementedException();
    public async Task RestartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public async Task StartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public async Task<ServerStatusResponse> StatusAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public async Task StopAsync(CancellationToken ct = default) => throw new NotImplementedException();
}
