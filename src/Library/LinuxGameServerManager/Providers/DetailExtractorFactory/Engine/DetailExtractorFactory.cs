using GameHost.Games.Lib.LinuxGameServerManager.Contracts.Response;
using GameHost.Games.Lib.LinuxGameServerManager.Providers.DetailExtratorFactory;
using Microsoft.Extensions.DependencyInjection;

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
    public ICollection<DetailsResponse> ProcessLines(string[] lines)
    {
        bool capture = false;
        DetailsResponse? lastEntry = default;
        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (line.Contains("======") && !capture)
            {
                capture = true;
                continue;
            }
            else if (line.Contains("======") && !capture)
            {
                if (lastEntry != default) _extracted.Remove(lastEntry);
                continue;
            }
            DetailsResponse? nextEntry = default;

            foreach (var provider in _resolvedProviders)
            {
                var result = provider.Process(trimmed);
                if (result != default)
                {
                    nextEntry = result;
                    break;
                }
            }
            if (nextEntry == default)
                nextEntry = new(trimmed, Contracts.Response.Enums.DetailType.Unknown);
            _extracted.Add(nextEntry);
            lastEntry = nextEntry;
        }
        return _extracted;
    }
}
