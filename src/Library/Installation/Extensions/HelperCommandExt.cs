using LunaticPanel.Core.Utils.Abstraction.LinuxCommand;
using Microsoft.Extensions.DependencyInjection;

namespace GameHost.Games.Lib.Installation.Extensions;

internal static class HelperCommandExt
{
    public static async Task<bool> CheckUsernameExist(this IServiceProvider serviceProvider, CancellationToken ct = default)
    {
        var linuxCommand = serviceProvider.GetRequiredService<ILinuxCommand>();
        var result = await linuxCommand
            .BuildCommand($"id -u {BaseInfo.USERNAME} >/dev/null")
            .AndPrintPayload(bool.TrueString)
            .OrPrintPayload(bool.FalseString)
            .ExecPayloadAsync<bool>();
        return result;
    }


    public static async Task<bool> CheckDependency(this IServiceProvider serviceProvider, string[] deps, CancellationToken ct = default)
    {
        var linuxCommand = serviceProvider.GetRequiredService<ILinuxCommand>();
        var result = await linuxCommand
    .BuildCommand($"dpkg -s {string.Join(' ', deps)} >/dev/null")
    .AndPrintPayload(bool.TrueString)
    .OrPrintPayload(bool.FalseString)
    .ExecPayloadAsync<bool>();
        return result;
    }

}
