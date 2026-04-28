using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response;
using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.Enums;
using GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtratorFactory;

namespace GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtractorFactory.Providers;

internal class PortExtractorProvider : IDetailExtractorProvider<PortExtractorProvider>
{
    public DetailsResponse? Process(string line)
    {
        bool port =
            (line.Contains("INBOUND", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("OUTBOUND", StringComparison.OrdinalIgnoreCase)) && (line.Contains("tcp", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("udp", StringComparison.OrdinalIgnoreCase));
        if (!port) return default;
        return new(line, port ? DetailType.Port : DetailType.Unknown);
    }
}
