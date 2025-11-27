using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.HeadersStrategy;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;

public class OperacaoService : IOperacaoService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly IEnvironmentStore _environmentStore;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IAmbientDataCallOptionsResolver _ambientDataCallOptionsResolver;

    public OperacaoService(IApiClientCallBuilder apiClientCallBuilder, IEnvironmentStore environmentStore,
        ICurrentEnvironment currentEnvironment, IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _environmentStore = environmentStore;
        _currentEnvironment = currentEnvironment;
        _ambientDataCallOptionsResolver = ambientDataCallOptionsResolver;
    }

    private const string ServiceName = "Korp.Producao.Apontamento";
    
    public async Task<bool> ValidarOdfPossuiApontamento(int numeroOdf)
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);

        var basePath = GetBasePath(environmentDetails.DesktopDatabaseVersion);
        
        var headerStrategy =
            new DatabaseNameHttpHeaderStrategy(_ambientDataCallOptionsResolver, environmentDetails.DatabaseName);
        
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{basePath}/validacao-odf-possui-apontamento?Odf={numeroOdf}")
            .WithHttpHeaderStrategy(headerStrategy)
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<bool>();

        return result;
    }
    
    public async Task<OperacaoDto> GetByNumeroOdfENumeroOperacao(int numeroOdf, string numeroOperacao)
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);

        var endpoint =
            $"{GetBasePath(environmentDetails.DesktopDatabaseVersion)}/operacaoordemproducao?odf={numeroOdf}&operacao={numeroOperacao}";

        var headerStrategy =
            new DatabaseNameHttpHeaderStrategy(_ambientDataCallOptionsResolver, environmentDetails.DatabaseName);

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(endpoint)
            .WithHttpHeaderStrategy(headerStrategy)
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<OperacaoDto>();

        return output;
    }
    
    public async Task<ApontamentoOperacaoOutput> GetApontamentoOperacaoByLegacyIdOperacao(int legacyIdOperacao)
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);

        var endpoint =
            $"{GetBasePath(environmentDetails.DesktopDatabaseVersion)}?IdOperacao={legacyIdOperacao}";

        var headerStrategy =
            new DatabaseNameHttpHeaderStrategy(_ambientDataCallOptionsResolver, environmentDetails.DatabaseName);

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(endpoint)
            .WithHttpHeaderStrategy(headerStrategy)
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<ApontamentoOperacaoOutput>();

        return output;
    }
    
    private string GetBasePath(string desktopDatabaseVersion)
    {
        return $"{desktopDatabaseVersion}/producao/apontamento";
    }
}