using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response;
using GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtratorFactory;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtractorFactory.Engine;

internal class DetailExtractorFactory : IDetailExtratorFactory
{
    private readonly DetailExtractorFactoryOptions _detailExtractorFactoryOptions;
    private ICollection<DetailsResponse> _extracted;
    private List<IDetailExtractorProvider> _resolvedProviders = new List<IDetailExtractorProvider>();

    public DetailExtractorFactory(IServiceProvider sp, DetailExtractorFactoryOptions detailExtractorFactoryOptions)
    {
        _extracted = new List<DetailsResponse>();
        _detailExtractorFactoryOptions = detailExtractorFactoryOptions;
        ResolvProviders(sp);

    }
    private void ResolvProviders(IServiceProvider sp)
    {
        foreach (var provider in _detailExtractorFactoryOptions.Providers)
        {
            _resolvedProviders.Add((IDetailExtractorProvider)sp.GetRequiredService(provider));
        }
    }

    private string CleanLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return line;
        var output = line;
        // 1. Strip ANSI Color/Escape Codes (e.g., \x1B[31m)
        string noColor = Regex.Replace(output, @"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])", "");

        // 2. Strip Control Characters [\x00-\x1F\x7F]
        output = Regex.Replace(noColor, @"[\x00-\x1F\x7F]", "");
        return output;
    }
    public ICollection<DetailsResponse> ProcessLines(string[] lines)
    {
        foreach (var line in lines)
        {
            var cleanLine = CleanLine(line);
            if (cleanLine.Contains("======")) continue;
            DetailsResponse? nextEntry = default;
            foreach (var provider in _resolvedProviders)
            {
                var result = provider.Process(cleanLine);
                if (result != default)
                {
                    nextEntry = result;
                    break;
                }
            }
            if (nextEntry == default)
                nextEntry = new(cleanLine, Contracts.Response.Enums.DetailType.Unknown);
            _extracted.Add(nextEntry);
        }
        return _extracted;
    }
}
