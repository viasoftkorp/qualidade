using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Services;

public class ConfiguracaoGeralService : IConfiguracaoGeralService, ITransientDependency
{
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/configuracoes-gerais";
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    public ConfiguracaoGeralService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<HttpResponseMessage> Get()
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;    
    }

    public async Task<HttpResponseMessage> Update(ConfiguracaoGeralInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;    
    }
}