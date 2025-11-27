using System;
using Viasoft.Core.Environment;
using Viasoft.Core.ServiceDiscovery.Consul;

namespace Viasoft.Qualidade.RNC.Core.Host.FrontendUrls;

public class FrontendUrl : IFrontendUrl
{
    private const string FrontEndUrlConsulKey = "FrontendUrl";
    public string Value { get; } = "";
    private static readonly Lazy<ConsulConfigurationProvider> Instance = new(() => new  ConsulConfigurationProvider(EnvironmentHelper.UriConsul(), "Global"));

    
    public FrontendUrl()
    {
        Instance.Value.Load();
        var hasKey = Instance.Value.TryGet(FrontEndUrlConsulKey, out var frontEndUrl);
        if (hasKey)
        {
            Value = frontEndUrl;
        }
    }
}