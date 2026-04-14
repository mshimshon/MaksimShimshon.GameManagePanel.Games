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
            if (parseResult.GetValue<bool>("--install"))
                await ExecuteCommandAsync(async () => await serviceServerInstallation.InstallAsync(ct));
            else if (parseResult.GetValue<bool>("--update"))
                await ExecuteCommandAsync(async () => await serviceServerInstallation.UpdateAsync(ct));
            else if (parseResult.GetValue<bool>("--version"))
                await ExecuteCommandAsync(async () => await serviceServerInstallation.GetVersionAsync(ct));
            else if (parseResult.GetValue<bool>("--check-update"))
                await ExecuteCommandAsync(async () => await serviceServerInstallation.CheckUpdateAsync(ct));
            else
                command.PrintHelp();
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
            .BuildCommand($"apt install -y {string.Join(' ', BaseInfo.dependencies)}")
            .ExecAsync();
            if (resultDepInstall.Failed)
                throw new InstallDependenciesFailedException(resultDepInstall.StandardError);

        });
        return command;
    }


}
