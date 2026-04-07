namespace MaksimShimshon.GameManagePanel.Lib.Installation;

internal interface IServerInstallation
{
    Task InstallAsync(CancellationToken ct = default);
    Task<bool> CheckUpdateAsync(CancellationToken ct = default);
    Task UpdateAsync(CancellationToken ct = default);
}
