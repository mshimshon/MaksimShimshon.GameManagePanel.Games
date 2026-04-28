using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using static GameHost.Games.Lib.Installation.Extensions.CommandLineExt;

namespace GameHost.Games.Lib.Installation.Extensions;

internal static class ServerCommandHandlerExt
{
    internal static Command SetServerAction(this Command command, IServiceProvider serviceProvider)
    {
        command.SetAction(async (parseResult, ct) =>
        {
            var serviceServerControl = serviceProvider.GetRequiredService<IServerControl>();
            bool success = false;
            if (parseResult.GetValue<bool>("--start"))
                success = await ExecuteCommandAsync(() => { _ = serviceServerControl.StartAsync(ct); return Task.CompletedTask; });
            else if (parseResult.GetValue<bool>("--stop"))
                success = await ExecuteCommandAsync(() => { _ = serviceServerControl.StopAsync(ct); return Task.CompletedTask; });
            else if (parseResult.GetValue<bool>("--restart"))
                success = await ExecuteCommandAsync(() => { _ = serviceServerControl.RestartAsync(ct); return Task.CompletedTask; });
            else if (parseResult.GetValue<bool>("--status"))
                success = await ExecuteCommandResultAsync(async () => await serviceServerControl.StatusAsync(ct));
            else
                await command.PrintHelp();
            if (success)
                Environment.Exit(0);
            else
                Environment.Exit(1);
        });
        return command;
    }
}