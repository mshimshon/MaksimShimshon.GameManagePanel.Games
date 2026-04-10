using LunaticPanel.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace GameHost.Games.Lib.LinuxGameServerManager;

public static class RegisterServicesExt
{
    public static void AddLinuxGameServerManagerServices(this IServiceCollection services)
    {
        services.AddLinuxCommandUtilityService();
    }

}
