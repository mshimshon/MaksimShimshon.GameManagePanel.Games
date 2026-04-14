using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameHost.Games.Lib.Installation.Services;

internal class PayloadBuilderService
{
    private JsonSerializerOptions _jsonSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
    public Task PrintAsync(object payload, CancellationToken ct = default)
    {
        string json = JsonSerializer.Serialize(payload, _jsonSerializerOptions);
        string payloadString = $"<<<PAYLOAD_BEGIN>>>\n{json}\n<<<PAYLOAD_END>>>";
        Console.Out.WriteLine(payloadString);
        return Task.CompletedTask;
    }
}
