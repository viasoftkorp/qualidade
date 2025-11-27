using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Company.Store;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OperacoesOrdemProducao.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.HeadersStrategy;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OperacoesOrdemProducao.Providers;

public class OperacaoOrdemProducaoProvider : IOperacaoOrdemProducaoProvider, ITransientDependency
{
    private const string ServiceName = "Korp.Producao.Apontamento";

    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly IOrdemProducaoProviderAclService _ordemProducaoProviderAclService;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IEnvironmentStore _environmentStore;
    private readonly IAmbientDataCallOptionsResolver _ambientDataCallOptionsResolver;
    private readonly ICompanyStore _companyStore;
    private readonly ICurrentCompany _currentCompany;
    private readonly ILogger<OperacaoOrdemProducaoProvider> _logger;

    public OperacaoOrdemProducaoProvider(IApiClientCallBuilder apiClientCallBuilder,
        IOrdemProducaoProviderAclService ordemProducaoProviderAclService,
        ICurrentEnvironment currentEnvironment, IEnvironmentStore environmentStore,
        IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver, ICompanyStore companyStore,
        ICurrentCompany currentCompany, ILogger<OperacaoOrdemProducaoProvider> logger)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _ordemProducaoProviderAclService = ordemProducaoProviderAclService;
        _currentEnvironment = currentEnvironment;
        _environmentStore = environmentStore;
        _ambientDataCallOptionsResolver = ambientDataCallOptionsResolver;
        _companyStore = companyStore;
        _currentCompany = currentCompany;
        _logger = logger;
    }

    public async Task<PagedResultDto<OperacaoOrdemProducaoDto>> GetList(GetListOrdemProducaoInput input)
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);
        var companyDetails = await _companyStore.GetCompanyDetailsAsync(_currentCompany.Id.ToString());

        var endpoint =
            $"{GetBaseEndpoint(environmentDetails.DesktopDatabaseVersion)}?Skip={input.SkipCount}" +
            $"&PageSize={input.MaxResultCount}&Odf={input.NumeroOdf}" +
            $"&IdEmpresa={companyDetails.LegacyCompanyId}" +
            $"&Partida={0}" +
            $"&advancedFilter={input.AdvancedFilter}";

        var headerStrategy =
            new DatabaseNameHttpHeaderStrategy(_ambientDataCallOptionsResolver, environmentDetails.DatabaseName);

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(endpoint)
            .WithHttpHeaderStrategy(headerStrategy)
            .WithHttpMethod(HttpMethod.Get)
            .DontThrowOnFailureCall()
            .Build();

        var response = await callBuilder.CallAsync<GetOperacaoOrdemProducaoDto>();
        GetOperacaoOrdemProducaoDto result;
        if (!response.IsSuccessStatusCode)
        {
            if (response.HttpResponseMessage.StatusCode != HttpStatusCode.BadRequest)
            {
                throw response.ErrorException;
            }

            result = await response.GetResponse();
            _logger.LogWarning($"Falha ao buscar operacoes: {result.Message}");

            return new PagedResultDto<OperacaoOrdemProducaoDto>
            {
                Items = new List<OperacaoOrdemProducaoDto>(),
                TotalCount = 0,
            };
        }

        result = await response.GetResponse();
        return new PagedResultDto<OperacaoOrdemProducaoDto>
        {
            Items = result.Operacoes,
            TotalCount = result.TotalCount,
        };
    }

    private string GetBaseEndpoint(string desktopDatabaseVersion)
    {
        var baseEndpoint = $"{desktopDatabaseVersion}/producao/apontamento/operacaoesordemproducao";
        return baseEndpoint;
    }
}