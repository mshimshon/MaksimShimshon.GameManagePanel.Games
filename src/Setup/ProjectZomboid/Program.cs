using GameHost.Games.Lib.Installation;
using GameHost.Games.Lib.LinuxGameServerManager;
using GameHost.Games.ProjectZomboid.Console.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = new ConsoleApplicationBuilder(args);
builder.Services.AddScoped<IServerInstallation, ServerInstallationService>();
builder.Services.AddScoped<IServerControl, ServerControlService>();
builder.Services.AddLinuxGameServerManagerServices();
var app = builder.Build();

await app.RunAsync();
