using GameHost.Games.Lib.Installation.Contracts.Responses;
using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;

namespace GameHost.Games.Lib.Installation;

public interface IServerInstallation
{
    Task InstallAsync(CancellationToken ct = default);
    Task<ServerUpdateResponse?> CheckUpdateAsync(CancellationToken ct = default);
    Task UpdateAsync(CancellationToken ct = default);
    Task<VersionResponse?> GetVersionAsync(CancellationToken ct = default);
    //Task GetGameInfoAsync(CancellationToken ct = default); // TODO: WRITE DIRECTLY INTO INSTALL LIB
    Task InstallDependenciesAsync(CancellationToken ct = default);
}
