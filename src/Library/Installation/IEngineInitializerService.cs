namespace GameHost.Games.Lib.Installation;

internal interface IEngineInitializerService
{
    Task InitializeAsync(CancellationToken ct = default);
}
