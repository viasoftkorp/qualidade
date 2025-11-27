using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.HeadersStrategy;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

public class OrdemProducaoProvider : IOrdemProducaoProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly IOrdemProducaoProviderAclService _ordemProducaoProviderAclService;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IEnvironmentStore _environmentStore;
    private readonly IAmbientDataCallOptionsResolver _ambientDataCallOptionsResolver;
    private const string ServiceName = "Korp.Producao.Apontamento";
    public OrdemProducaoProvider(IApiClientCallBuilder apiClientCallBuilder,
        IOrdemProducaoProviderAclService ordemProducaoProviderAclService,
        ICurrentEnvironment currentEnvironment, IEnvironmentStore environmentStore,
        IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _ordemProducaoProviderAclService = ordemProducaoProviderAclService;
        _currentEnvironment = currentEnvironment;
        _environmentStore = environmentStore;
        _ambientDataCallOptionsResolver = ambientDataCallOptionsResolver;
    }
    public async Task<PagedResultDto<OrdemProducaoOutput>> GetList(GetListOrdemProducaoInput input)
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);

        var endpoint = $"{GetBaseEndpoint(environmentDetails.DesktopDatabaseVersion)}?Skip={input.SkipCount}&PageSize={input.MaxResultCount}&Filter={input.NumeroOdf}";
        if (input.FiltrarApenasEmitidas)
        {
            endpoint += "&Emitida=true";
        }
        else
        {
            endpoint += "&Emitida=false";
        }
        var headerStrategy =
            new DatabaseNameHttpHeaderStrategy(_ambientDataCallOptionsResolver, environmentDetails.DatabaseName);

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(endpoint)
            .WithHttpHeaderStrategy(headerStrategy)
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<GetOrdemProducaoDtoRetorno>();
        if (result.TotalCount > 0)
        {
            var itens = await _ordemProducaoProviderAclService.ProcessGetList(result.Ordens);
            return new PagedResultDto<OrdemProducaoOutput>
            {
                Items = itens,
                TotalCount = result.TotalCount
            };
        }

        return new PagedResultDto<OrdemProducaoOutput>();
    }

    private string GetBaseEndpoint(string desktopDatabaseVersion)
    {
        var baseEndpoint = $"{desktopDatabaseVersion}/producao/apontamento/ordens-producao";
        return baseEndpoint;
    }
    
    private class GetOrdemProducaoDtoRetorno
    {
        public List<OrdemProducao> Ordens { get; set; }
        public long TotalCount { get; set; }
    }
}