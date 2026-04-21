using GameHost.Games.Lib.Installation.Optionals;
using GameHost.Games.Lib.Installation.Services;
using LunaticPanel.Core.Utils;
using LunaticPanel.Core.Utils.Logging;
using Microsoft.Extensions.DependencyInjection;
using static GameHost.Core.BaseInfo;

namespace GameHost.Games.Lib.Installation;

public static class ServiceRegistrationExt
{
    internal static void AddInstallationServices(this IServiceCollection services)
    {
        services.AddScoped<IServerModControl, DefaultServerModControlService>();
        services.AddPluginLocationUtilityService(AssemblyName);
        services.AddLinuxCommandUtilityService();
        services.AddCrazyReportUtilityService();
        services.AddScoped<ICrazyReportCircuit, CrazyReportCircuit>();
    }
}
