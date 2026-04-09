namespace GameHost.Games.Lib.Installation.Optional;

/// <summary>
/// This is an optional service and if it's not implemented and registered it is then not available
/// </summary>
public interface IServerModControl
{
    Task<bool> HasModSupportAsync(CancellationToken ct = default);
    Task ProcessModListAsync(CancellationToken ct = default);
    Task CleanModListAsync(CancellationToken ct = default);

}
