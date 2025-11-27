using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Core.Gateway;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.TenantDbDiscovery;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;

public class ExternalOrdemRetrabalhoService : IExternalOrdemRetrabalhoService, ITransientDependency
{
    private readonly ITenantDbDiscoveryService _tenantDbDiscoveryService;
    private readonly IEnvironmentStore _environmentStore;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUserStore _userStore;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentCompany _currentCompany;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExternalOrdemRetrabalhoService> _logger;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGatewayUriProvider _gatewayProvider;
    private readonly IAmbientData _ambientData;

    public ExternalOrdemRetrabalhoService(ITenantDbDiscoveryService tenantDbDiscoveryService,
        IEnvironmentStore environmentStore, ICurrentEnvironment currentEnvironment, IUserStore userStore,
        ICurrentUser currentUser, ICurrentCompany currentCompany, IHttpClientFactory httpClientFactory,
        ILogger<ExternalOrdemRetrabalhoService> logger, ICurrentTenant currentTenant, IGatewayUriProvider gatewayProvider, IAmbientData ambientData)
    {
        _tenantDbDiscoveryService = tenantDbDiscoveryService;
        _environmentStore = environmentStore;
        _currentEnvironment = currentEnvironment;
        _userStore = userStore;
        _currentUser = currentUser;
        _currentCompany = currentCompany;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _currentTenant = currentTenant;
        _gatewayProvider = gatewayProvider;
        _ambientData = ambientData;
    }

    public async Task<ExternalGerarOrdemRetrabalhoOutput> GerarOrdemRetrabalho(ExternalGerarOrdemRetrabalhoInput input)
    {
        var body = JsonSerializer.Serialize(input,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var resultRequest = await SendRequest(body, false);
        var responseString = await resultRequest.Content.ReadAsStringAsync();
        if (!resultRequest.IsSuccessStatusCode)
        {
            var output = new ExternalGerarOrdemRetrabalhoOutput
            {
                Message = "Erro ao gerar Ordem de Retrabalho, status Code: " + resultRequest.StatusCode +
                          ". Erro Original: " + responseString,
                Success = false
            };

            _logger.LogError($"{output.Message} " +
                             $"Dto enviado: {JsonConvert.SerializeObject(input)}");
            return output;
        }

        return JsonSerializer.Deserialize<ExternalGerarOrdemRetrabalhoOutput>(responseString,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    public async Task<OrdemRetrabalhoNaoConformidadeOutput> EstornarOrdemRetrabalho(ExternalEstornarOrdemRetrabalhoInput input)
    {
        var body = JsonSerializer.Serialize(input,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var resultRequest = await SendRequest(body, true);
        var responseString = await resultRequest.Content.ReadAsStringAsync();

        if (!resultRequest.IsSuccessStatusCode)
        {
            var output = new OrdemRetrabalhoNaoConformidadeOutput
            {
                Message = "Erro ao estornar Ordem de Retrabalho, status Code: " + resultRequest.StatusCode +
                          ". Erro Original: " + responseString,
                Success = false
            };

            _logger.LogError($"{output.Message} " +
                             $"Dto enviado: {JsonConvert.SerializeObject(input)}");

            return output;
        }

        return new OrdemRetrabalhoNaoConformidadeOutput
        {
            Message = "",
            Success = true
        };
    }

    private async Task<HttpResponseMessage> SendRequest(string body, bool estorno)
    {
        var endpoint = await GetEndpoint();
        var currentUser = await _userStore.GetUserDetailsAsync(_ambientData.GetUserId());

        using (var client = _httpClientFactory.CreateClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            if (estorno)
            {
                request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
            }

            request.Headers.Add("Usuario", currentUser.Login);
            request.Headers.Add("IdEmpresa", _currentCompany.LegacyId.ToString());
            request.Headers.Add("Accept", "application/json;v2");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var timer = new Stopwatch();
            timer.Start();
            var erpResponse = await client.SendAsync(request);
            timer.Stop();
            if (estorno)
            {
                _logger.LogWarning(
                    $"Estorno de ODF de retrabalho Levou {timer.ElapsedMilliseconds} milisegundos para o tenant {_currentTenant.Id}");
            }
            else
            {
                _logger.LogWarning(
                    $"Geração de ODF de retrabalho Levou {timer.ElapsedMilliseconds} milisegundos para o tenant {_currentTenant.Id}");
            }

            timer.Reset();
            return erpResponse;
        }
    }

    private async Task<string> GetEndpoint()
    {
        var gatewayUrl = _gatewayProvider.GetGatewayUri().ToString();
        var environmentDetails = await _environmentStore.GetEnvironmentAsync( _ambientData.GetEnvironmentId());

        return
            $"{gatewayUrl}korp/services/{environmentDetails.DesktopDatabaseVersion}/Logistica/{environmentDetails.DatabaseName}/ordem-producao";
    }
}