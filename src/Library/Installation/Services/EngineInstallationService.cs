using GameHost.Core.Features;
using GameHost.Games.Lib.Installation.Contracts.Responses;
using GameHost.Games.Lib.Installation.Contracts.Responses.ValueObjects;
using GameHost.Games.Lib.Installation.Exceptions;
using LunaticPanel.Core.Utils.Abstraction.Plugin.Location;
using LunaticPanel.Core.Utils.Abstraction.SafeFileWriter;
using System.Text.Json;

namespace GameHost.Games.Lib.Installation.Services;

internal class EngineInstallationService : IEngineInstallation
{
    private readonly IServerInstallation _serverInstallation;
    private readonly ISafeFileWriter _safeFileWriter;
    private readonly IPluginSystemLocation _pluginSystemLocation;
    private readonly IEngineInitializerService _engineInitializerService;
    private readonly IMetadataService _metadataService;


    public EngineInstallationService(IServerInstallation serverInstallation,
        ISafeFileWriter safeFileWriter,
        IPluginLocation pluginLocation,
        IEngineInitializerService engineInitializerService,
        IMetadataService metadataService)
    {
        _safeFileWriter = safeFileWriter;
        _serverInstallation = serverInstallation;
        _pluginSystemLocation = pluginLocation;
        _engineInitializerService = engineInitializerService;
        _metadataService = metadataService;
    }



    public async Task InstallAsync(Func<string, CancellationToken, Task> updateProgressStatus, CancellationToken ct = default)
    {

        string installStateFile = _pluginSystemLocation.GetConfigFor(LinuxGameServerKeys.MODULE_NAME, LinuxGameServerKeys.SERVER_INSTALL_STATE_FILE);
        if (File.Exists(installStateFile))
            throw new ServerAlreadyInstalledException();
        string installProgressFile = _pluginSystemLocation.GetConfigFor(LinuxGameServerKeys.MODULE_NAME, LinuxGameServerKeys.SERVER_INSTALL_PROGRESS_FILE);
        var manifest = await _metadataService.GetManifestAsync(ct);
        var progress = new InstallationProgressResponse()
        {
            CurrentStep = $"Installing {manifest.DisplayName}...", // TODO: LOCALIZe
            DisplayName = manifest.DisplayName,
            Id = manifest.Id,
            IsInstalling = true
        };
        try
        {
            string json = JsonSerializer.Serialize(progress, BaseInfo.jsonSerializerOptions);
            await _safeFileWriter.WriteThenCopyFileAsync(installProgressFile, json);
            await _serverInstallation.InstallAsync(async (status, token) =>
            {
                var toWrite = progress with { CurrentStep = status };
                string toWriteJson = JsonSerializer.Serialize(toWrite, BaseInfo.jsonSerializerOptions);
                await _safeFileWriter.WriteThenCopyFileAsync(installProgressFile, json);
            }, ct);
            var installState = new InstallationStateResponse()
            {
                Id = manifest.Id,
                InstallDate = DateTime.UtcNow
            };
            string installStateJson = JsonSerializer.Serialize(installState, BaseInfo.jsonSerializerOptions);
            await _safeFileWriter.WriteThenCopyFileAsync(installStateFile, installStateJson);
            await PostInstallAsync();
        }
        catch
        {
            progress = progress with
            {
                CurrentStep = $"Failed to Install {manifest.DisplayName}", // TODO: LOCALIZe
                IsInstalling = false,
                FailureReason = "Failed to install the game server check log." // TODO: LOCALIZe
            };
            string json = JsonSerializer.Serialize(progress, BaseInfo.jsonSerializerOptions);
            await _safeFileWriter.WriteThenCopyFileAsync(installProgressFile, json);

            throw;
        }

    }
    public Task<ServerUpdateResponse?> CheckUpdateAsync(CancellationToken ct = default) => _serverInstallation.CheckUpdateAsync(ct);
    public Task UpdateAsync(CancellationToken ct = default) => _serverInstallation.UpdateAsync(ct);
    public Task<VersionResponse?> GetVersionAsync(CancellationToken ct = default) => _serverInstallation.GetVersionAsync(ct);
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        await _engineInitializerService.InitializeAsync(ct);
        await _serverInstallation.InitializeAsync(ct);

    }

    public async Task PostInstallAsync(CancellationToken ct = default)
    {

        // ServerIdentityResponse
        await _serverInstallation.PostInstallAsync(ct);
    }

}
