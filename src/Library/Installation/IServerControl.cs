namespace MaksimShimshon.GameManagePanel.Lib.Installation;

internal interface IServerControl
{
    Task StartAsync(CancellationToken ct = default);
    Task StopAsync(CancellationToken ct = default);
    Task RestartAsync(CancellationToken ct = default);
    Task StatusAsync(CancellationToken ct = default);
}
