using GameHost.Games.Lib.Installation;
using GameHost.Games.Lib.Installation.Contracts.Responses;

namespace GameHost.Games.ProjectZomboid.Console.Services;

internal class ServerControlService : IServerControl
{
    public Task ConsoleAsync(Func<string, Task> consoleStream, CancellationToken ct = default) => throw new NotImplementedException();
    public Task RestartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task StartAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task<ServerStatusResponse> StatusAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task StopAsync(CancellationToken ct = default) => throw new NotImplementedException();
}
