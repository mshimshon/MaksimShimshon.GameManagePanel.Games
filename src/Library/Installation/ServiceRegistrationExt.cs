using GameHost.Games.Lib.Installation.Optionals;
using GameHost.Games.Lib.Installation.Services;
using LunaticPanel.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using static MaksimShimshon.GameManagePanel.Core.BaseInfo;

namespace GameHost.Games.Lib.Installation;

public static class ServiceRegistrationExt
{
    internal static void AddInstallationServices(this IServiceCollection services)
    {
        services.AddScoped<IServerModControl, DefaultServerModControlService>();
        services.AddPluginLocationUtilityService(AssemblyName);
        services.AddLinuxCommandUtilityService();
        services.AddCrazyReportUtilityService();
    }
}
