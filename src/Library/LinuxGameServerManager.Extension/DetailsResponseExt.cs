using GameHost.Games.Lib.Installation.Contracts.Responses.Status;
using GameHost.Games.Lib.Installation.Contracts.Responses.Status.Enums;
using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response;
using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response.Enums;
using System.Text.RegularExpressions;

namespace LinuxGameServerManager.Extension;

public static class DetailsResponseExt
{

    private static void FilterForStatus(DetailsResponse detail, ServerStatusResponse serverStatus)
    {
        if (detail.DetailType != DetailType.Status) return;
        StringComparison comparer = StringComparison.OrdinalIgnoreCase;
        bool isStarted = detail.Raw.Contains("started", comparer) || detail.Raw.Contains("online", comparer);
        bool isStopped = detail.Raw.Contains("stopped", comparer) || detail.Raw.Contains("offline", comparer);
        if (isStarted) serverStatus.Status = ServerStatus.Started;
        else if (isStopped) serverStatus.Status = ServerStatus.Started;
    }

    private static void FilterForIP(DetailsResponse detail, ServerStatusResponse serverStatus)
    {
        if (detail.DetailType != DetailType.IP || !detail.HasKeyPair) return;
        StringComparison comparer = StringComparison.OrdinalIgnoreCase;
        var matches = Regex.Matches(detail.Raw, @"([^\s]+)");
        var matchesList = matches.ToList();
        string title = matches.First().Value;
        matchesList.RemoveAt(0);
        string direction = "unknown";
        string? protocol = default;
        string? port = default;
        foreach (Match m in matchesList)
        {
            if (m.Value.Contains("INBOUND", comparer) || m.Value.Contains("Outbound", comparer))
            {
                direction = m.Value;
            }
            else if (Regex.IsMatch(m.Value, @"^(?:\d+|\d+-\d+|\d+(?:,\d+)+|\d+:\d+)$"))
            {
                port = m.Value;
            }
            else if (m.Value.Contains("tcp", comparer) || m.Value.Contains("udp", comparer))
            {
                protocol = m.Value;
            }
        }
        PortInfoResponse? portInfo = default;
        if (protocol != default && port != default)
            portInfo = new()
            {
                Name = title,
                Protocol = protocol,
                Port = port
            };
        if (portInfo == default) return;
        if (serverStatus.ConnectionInfo == default)
            serverStatus.ConnectionInfo = new()
            {
                PortInfoResponses = new() { portInfo }
            };
        else
            serverStatus.ConnectionInfo.PortInfoResponses.Add(portInfo);
    }

    private static void FilterForPort(DetailsResponse detail, ServerStatusResponse serverStatus)
    {
        if (detail.DetailType != DetailType.IP || !detail.HasKeyPair) return;
        if (serverStatus.ConnectionInfo == default)
            serverStatus.ConnectionInfo = new() { Address = detail.Value! };
        else
            serverStatus.ConnectionInfo.Address = detail.Value!;
    }
    public static ServerStatusResponse ConvertToServerStatus(this IReadOnlyCollection<DetailsResponse> details)
    {
        ServerStatusResponse result = new()
        {
            Status = ServerStatus.Unknown
        };

        foreach (var detail in details)
        {
            FilterForStatus(detail, result);
            FilterForIP(detail, result);
            FilterForPort(detail, result);
        }
        return result;
    }

}
