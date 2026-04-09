using GameHost.Games.Lib.Installation;
using GameHost.Games.Lib.Steam;
using LunaticPanel.Core.Utils;
using MaksimShimshon.GameManagePanel.Core;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");
IServiceCollection services = new ServiceCollection();
services.AddInstallationServices();
services.AddSteamServices();
services.AddPluginLocationUtilityService(BaseInfo.AssemblyName);

var provider = services.BuildServiceProvider().CreateScope().ServiceProvider;
var cts = new CancellationTokenSource();
await provider.RunStartupCommandAsync(cts.Token, args);