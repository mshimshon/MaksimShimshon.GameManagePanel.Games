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
            if (parseResult.GetValue<bool>("--start"))
                await ExecuteCommandAsync(() => serviceServerControl.StartAsync(ct));
            else if (parseResult.GetValue<bool>("--stop"))
                await ExecuteCommandAsync(() => serviceServerControl.StopAsync(ct));
            else if (parseResult.GetValue<bool>("--restart"))
                await ExecuteCommandAsync(() => serviceServerControl.RestartAsync(ct));
            else if (parseResult.GetValue<bool>("--status"))
                await ExecuteCommandResultAsync(async () => await serviceServerControl.StatusAsync(ct));
            else
                await command.PrintHelp();
        });
        return command;
    }
}