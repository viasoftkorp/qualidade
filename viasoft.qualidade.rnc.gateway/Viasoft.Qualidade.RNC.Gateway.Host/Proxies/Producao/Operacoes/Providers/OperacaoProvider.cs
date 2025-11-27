using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.Operacoes.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.HeadersStrategy;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.Operacoes.Providers;

public class OperacaoProvider : IOperacaoProvider, ITransientDependency
{
    private const string ServiceName = "Korp.Producao.Apontamento";

    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IEnvironmentStore _environmentStore;
    private readonly IAmbientDataCallOptionsResolver _ambientDataCallOptionsResolver;
    private readonly ILogger<OperacaoProvider> _logger;

    public OperacaoProvider(IApiClientCallBuilder apiClientCallBuilder,
        ICurrentEnvironment currentEnvironment, IEnvironmentStore environmentStore,
        IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver, ILogger<OperacaoProvider> logger)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentEnvironment = currentEnvironment;
        _environmentStore = environmentStore;
        _ambientDataCallOptionsResolver = ambientDataCallOptionsResolver;
        _logger = logger;
    }

    public async Task<OperacaoSaldoOutput> GetSaldo(int legacyIdOperacao)
    {
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);

        var endpoint =
            $"{GetBaseEndpoint(environmentDetails.DesktopDatabaseVersion)}?IdOperacao={legacyIdOperacao}";

        var headerStrategy =
            new DatabaseNameHttpHeaderStrategy(_ambientDataCallOptionsResolver, environmentDetails.DatabaseName);

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(endpoint)
            .WithHttpHeaderStrategy(headerStrategy)
            .WithHttpMethod(HttpMethod.Get)
            .DontThrowOnFailureCall()
            .Build();

        var response = await callBuilder.CallAsync<GetApontamentoOperacaoOutput>();
        GetApontamentoOperacaoOutput responseOutput;
        if (!response.IsSuccessStatusCode)
        {
            if (response.HttpResponseMessage.StatusCode != HttpStatusCode.BadRequest)
            {
                throw response.ErrorException;
            }

            responseOutput = await response.GetResponse();
            _logger.LogWarning($"Falha ao buscar operacao saldo para operacao {legacyIdOperacao}: {responseOutput.Message}");
            return new OperacaoSaldoOutput();
        }

        responseOutput = await response.GetResponse();
        return new OperacaoSaldoOutput(responseOutput);
    }

    private string GetBaseEndpoint(string desktopDatabaseVersion)
    {
        var baseEndpoint = $"{desktopDatabaseVersion}/producao/apontamento";
        return baseEndpoint;
    }
}