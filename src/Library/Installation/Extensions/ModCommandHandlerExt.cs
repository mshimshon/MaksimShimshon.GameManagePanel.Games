using GameHost.Games.Lib.Installation.Optionals;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using static GameHost.Games.Lib.Installation.Extensions.CommandLineExt;
namespace GameHost.Games.Lib.Installation.Extensions;

internal static class ModCommandHandlerExt
{
    internal static Command SetModAction(this Command command, IServiceProvider serviceProvider)
    {
        command.SetAction(async (parseResult, ct) =>
        {
            var serviceModControl = serviceProvider.GetRequiredService<IServerModControl>();
            bool success = false;
            if (parseResult.GetValue<bool>("--check"))
                success = await ExecuteCommandResultAsync(async () => await serviceModControl.HasModSupportAsync(ct));
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
