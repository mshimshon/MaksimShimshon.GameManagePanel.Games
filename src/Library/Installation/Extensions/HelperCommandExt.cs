using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;
using LunaticPanel.Core.Utils.Abstraction.LinuxCommand.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace GameHost.Games.Lib.Installation.Extensions;

internal static class HelperCommandExt
{
    public static async Task<bool> CheckUsernameExist(this IServiceProvider serviceProvider, CancellationToken ct = default)
        => await CommandLineExt.ExecuteCommandAsync(async () =>
        {
            var linuxCommand = serviceProvider.GetRequiredService<ILinuxCommand>();
            return linuxCommand.BoolCommandAsync($"id -u {BaseInfo.USERNAME} >/dev/null", ct);
        });

    public static async Task<bool> CheckDependency(this IServiceProvider serviceProvider, string[] deps, CancellationToken ct = default)
        => await CommandLineExt.ExecuteCommandAsync(async () =>
        {
            var linuxCommand = serviceProvider.GetRequiredService<ILinuxCommand>();
            return linuxCommand.BoolCommandAsync($"dpkg -s {string.Join(' ', deps)} >/dev/null", ct);
        });

}
