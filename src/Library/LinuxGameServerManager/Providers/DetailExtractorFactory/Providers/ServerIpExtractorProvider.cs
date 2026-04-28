using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response;
using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.Enums;
using GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtratorFactory;
using System.Text.RegularExpressions;

namespace GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtractorFactory.Providers;

internal class ServerIpExtractorProvider : IDetailExtractorProvider<ServerIpExtractorProvider>
{

    public DetailsResponse? Process(string line)
    {
        StringComparison comparer = StringComparison.OrdinalIgnoreCase;
        var match = Regex.Match(line, @"\b\d{1,3}(?:\.\d{1,3}){3}\b");
        bool detectedIp = match.Success && line.StartsWith("Server IP:", comparer);
        if (!detectedIp) return default;
        List<string> keyValue = line.Split(':').ToList();
        string key = keyValue[0].Trim();
        keyValue.RemoveAt(0);
        string value = string.Join(':', keyValue);
        return new DetailsResponse(line, key, value, DetailType.IP);
    }
}
