using GameHost.Games.Lib.Installation;

var builder = new ConsoleApplicationBuilder(args);
var app = builder.Build();

await app.RunAsync();
