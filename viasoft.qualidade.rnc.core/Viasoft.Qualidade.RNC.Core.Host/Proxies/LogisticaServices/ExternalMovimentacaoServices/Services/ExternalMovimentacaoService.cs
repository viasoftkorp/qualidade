using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Legacy;
using Viasoft.Qualidade.RNC.Core.Domain.TenantDbDiscovery;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;

public class ExternalMovimentacaoService : IExternalMovimentacaoService, ITransientDependency
{
    private readonly IUserStore _userStore;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentCompany _currentCompany;
    private readonly ITenantDbDiscoveryService _tenantDbDiscoveryService;
    private readonly IEnvironmentStore _environmentStore;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly ILogger<ExternalMovimentacaoService> _logger;
    private readonly ICurrentTenant _currentTenant;

    public ExternalMovimentacaoService(IUserStore userStore, IHttpClientFactory httpClientFactory, ICurrentUser currentUser,
        ICurrentCompany currentCompany, ITenantDbDiscoveryService tenantDbDiscoveryService, IEnvironmentStore environmentStore, 
        ICurrentEnvironment currentEnvironment, ILogger<ExternalMovimentacaoService> logger, ICurrentTenant currentTenant)
    {
        _userStore = userStore;
        _httpClientFactory = httpClientFactory;
        _currentUser = currentUser;
        _currentCompany = currentCompany;
        _tenantDbDiscoveryService = tenantDbDiscoveryService;
        _environmentStore = environmentStore;
        _currentEnvironment = currentEnvironment;
        _logger = logger;
        _currentTenant = currentTenant;
    }
    public async Task<ExternalMovimentarEstoqueItemOutput> MovimentarEstoqueLista(ExternalMovimentarEstoqueListaInput input)
    {
        var body = JsonSerializer.Serialize(input,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        _logger.LogInformation($"Dto enviado para a movimentação de estoque: {body}");

        var resultRequest = await SendRequest(body);
        var responseString = await resultRequest.Content.ReadAsStringAsync();
        _logger.LogInformation($"Retorno movimentação de estoque: {responseString}");

        if (!resultRequest.IsSuccessStatusCode)
        {
            return new ExternalMovimentarEstoqueItemOutput
            {
                Error = new KorpErro
                {
                    code = resultRequest.StatusCode.ToString(),
                    message = responseString
                }
            };
        }

        try
        {
            return JsonSerializer.Deserialize<ExternalMovimentarEstoqueItemOutput>(responseString,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
        catch (Exception e)
        {            
            _logger.LogWarning(e, "Erro ao deserializar retorno movimentação estoque");
            return new ExternalMovimentarEstoqueItemOutput
            {
                Error = new KorpErro()
            };
        }
    }
    
    private async Task<HttpResponseMessage> SendRequest(string body)
    {
        var endpoint = await GetEndpoint();
        var currentUser = await _userStore.GetUserDetailsAsync(_currentUser.Id);
        
        using (var client = _httpClientFactory.CreateClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            
            request.Headers.Add("Usuario", currentUser.Login);
            request.Headers.Add("IdEmpresa", _currentCompany.LegacyId.ToString());
            request.Headers.Add("Accept", "application/json;v2");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var timer = new Stopwatch();
            timer.Start();
            var erpResponse = await client.SendAsync(request);
            timer.Stop();
            _logger.LogWarning($"Movimentação de estoque Levou {timer.ElapsedMilliseconds} milisegundos para o tenant {_currentTenant.Id}");
            timer.Reset();
            return erpResponse;
        }
    }

    private async Task<string> GetEndpoint()
    {
        var serverIp = await _tenantDbDiscoveryService.ServerIp();
        
        var environmentDetails = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);
        return
            $"{serverIp}/korp/services/{environmentDetails.DesktopDatabaseVersion}/Logistica/{environmentDetails.DatabaseName}/wmsService/transferirEstoqueLista";
    }
}