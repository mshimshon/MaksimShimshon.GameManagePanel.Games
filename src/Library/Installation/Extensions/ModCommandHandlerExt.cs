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
            if (parseResult.GetValue<bool>("--check"))
                await ExecuteCommandAsync(async () => await serviceModControl.HasModSupportAsync(ct));

        });
        return command;
    }
}
