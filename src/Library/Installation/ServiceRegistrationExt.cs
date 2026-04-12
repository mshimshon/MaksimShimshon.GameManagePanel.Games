using GameHost.Games.Lib.Installation.Optional;
using GameHost.Games.Lib.Installation.Services;
using LunaticPanel.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace GameHost.Games.Lib.Installation;

public static class ServiceRegistrationExt
{
    public static void AddInstallationServices(this IServiceCollection services)
    {
        services.AddScoped<IServerModControl, DefaultServerModControlService>();
        services.AddLinuxCommandUtilityService();
        services.AddCrazyReportUtilityService();
    }




}
