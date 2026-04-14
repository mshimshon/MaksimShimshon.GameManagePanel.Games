using GameHost.Games.Lib.Installation.Optionals;
using GameHost.Games.Lib.Installation.Services;
using LunaticPanel.Core.Utils;
using MaksimShimshon.GameManagePanel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace GameHost.Games.Lib.Installation;

public static class ServiceRegistrationExt
{
    internal static void AddInstallationServices(this IServiceCollection services)
    {
        services.AddScoped<IServerModControl, DefaultServerModControlService>();
        services.AddPluginLocationUtilityService(BaseInfo.AssemblyName);
        services.AddLinuxCommandUtilityService();
        services.AddCrazyReportUtilityService();
    }
}
