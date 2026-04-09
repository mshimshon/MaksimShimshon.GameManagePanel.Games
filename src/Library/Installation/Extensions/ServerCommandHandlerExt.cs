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
                await ExecuteCommandAsync(async () => await serviceServerControl.StartAsync(ct));
            else if (parseResult.GetValue<bool>("--stop"))
                await ExecuteCommandAsync(async () => await serviceServerControl.StopAsync(ct));
            else if (parseResult.GetValue<bool>("--restart"))
                await ExecuteCommandAsync(async () => await serviceServerControl.RestartAsync(ct));
            else if (parseResult.GetValue<bool>("--status"))
                await ExecuteCommandAsync(async () => await serviceServerControl.StatusAsync(ct));
            else
                command.PrintHelp();
        });
        return command;
    }
}