using GameHost.Games.Lib.Installation.Contracts.Responses.Mods;

namespace GameHost.Games.Lib.Installation.Optionals;

/// <summary>
/// This is an optional service and if it's not implemented and registered it is then not available
/// </summary>
public interface IServerModControl
{
    Task<bool> IsSupportedAsync(CancellationToken ct = default);
    Task<ModFeatureResponse> GetDetailsAsync(CancellationToken ct = default);
    Task ProcessModListAsync(CancellationToken ct = default);
    Task CleanModListAsync(CancellationToken ct = default);

}
