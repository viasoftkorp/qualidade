using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;

public class OperacaoRetrabalhoProxyService : IOperacaoRetrabalhoProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly IEnvironmentStore _environmentStore;
    private readonly ICurrentEnvironment _currentEnvironment;
    private const string ServiceName = "Korp.Producao.Apontamento";
    private const string BasePath = "/producao/apontamento/ordem-producao/operacoes/retrabalhos";
    public OperacaoRetrabalhoProxyService(IApiClientCallBuilder apiClientCallBuilder, IEnvironmentStore environmentStore, 
        ICurrentEnvironment currentEnvironment)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _environmentStore = environmentStore;
        _currentEnvironment = currentEnvironment;
    }
    public async Task<GerarOperacaoRetrabalhoExternalOutput> Create(GerarOperacaoRetrabalhoExternalInput externalInput)
    {
        var endpoint = await GetEndpoint();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(endpoint)
            .WithBody(externalInput)
            .WithHttpMethod(HttpMethod.Post)
            .DontThrowOnFailureCall()
            .Build();

        var result = await callBuilder.ResponseCallAsync<GerarOperacaoRetrabalhoExternalOutput>();

        return result;
    }

    private async Task<string> GetEndpoint()
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);
        return
            $"{environmentDetails.DesktopDatabaseVersion}{BasePath}";
    }
}