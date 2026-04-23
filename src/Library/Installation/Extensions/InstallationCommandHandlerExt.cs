using GameHost.Games.Lib.Installation.Exceptions;
using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using static GameHost.Games.Lib.Installation.Extensions.CommandLineExt;

namespace GameHost.Games.Lib.Installation.Extensions;

internal static class InstallationCommandHandlerExt
{
    internal static Command SetInstallationAction(this Command command, IServiceProvider serviceProvider)
    {
        command.SetAction(async (parseResult, ct) =>
        {
            var serviceServerInstallation = serviceProvider.GetRequiredService<IServerInstallation>();
            bool success = false;
            if (parseResult.GetValue<bool>("--install"))
                success = await ExecuteCommandAsync(async () => await serviceServerInstallation.InstallAsync(ct));
            else if (parseResult.GetValue<bool>("--update"))
                success = await ExecuteCommandAsync(async () => await serviceServerInstallation.UpdateAsync(ct));
            else if (parseResult.GetValue<bool>("--version"))
                success = await ExecuteCommandResultAsync(async () => await serviceServerInstallation.GetVersionAsync(ct));
            else if (parseResult.GetValue<bool>("--check-update"))
                success = await ExecuteCommandResultAsync(async () => await serviceServerInstallation.CheckUpdateAsync(ct));
            else
                await command.PrintHelp();
            if (success)
                Environment.Exit(0);
            else
                Environment.Exit(1);
        });
        return command;
    }

    internal static Command SetInitializingAction(this Command command, IServiceProvider serviceProvider)
    {
        var linuxCommandService = serviceProvider.GetRequiredService<ILinuxCommand>();
        command.SetAction(async (parseResult, ct) =>
        {
            bool isUsernameCreated = await serviceProvider.CheckUsernameExist();
            if (!isUsernameCreated)
            {
                var resultUsercreate = await linuxCommandService
                .BuildCommand($"sudo adduser --home --system --shell /usr/sbin/nologin {BaseInfo.USERNAME} ")
                .ExecAsync();
                if (resultUsercreate.Failed)
                    throw new CreateUsernameFailedException(resultUsercreate.StandardError);
            }
            var resultDepInstall = await linuxCommandService
            .BuildCommand($"apt-get install -y {string.Join(' ', BaseInfo.dependencies)}")
            .ExecAsync();
            if (resultDepInstall.Failed)
                throw new InstallDependenciesFailedException(resultDepInstall.StandardError);

        });
        return command;
    }


}
