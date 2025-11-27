using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viasoft.Core.API.Reporting;
using Viasoft.Core.API.Reporting.Model;
using Viasoft.Core.ApiClient;
using Viasoft.Core.Authentication.Proxy.Dtos.Inputs;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company.Store;
using Viasoft.Core.Reporting.Model;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Extensions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.
    Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.
    Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Services;

public class RelatoriosProvider : IRelatoriosProvider, ITransientDependency
{
    private readonly IReportingApi _externalReportingService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUser _currentUser;
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ILogger<RelatoriosProvider> _logger;
    private readonly IUserStore _userStore;
    private readonly INaoConformidadeProvider _naoConformidadeProvider;
    private readonly ICausaNaoConformidadeProvider _causaNaoConformidadeProvider;
    private readonly ISolucaoNaoConformidadeProvider _solucaoNaoConformidadeProvider;
    private readonly IDefeitoNaoConformidadeProvider _defeitoNaoConformidadeProvider;
    private readonly IAcaoPreventivaNaoConformidadeProvider _acaoPreventivaNaoConformidadeProvider;
    private readonly ICentroCustoCausaNaoConformidadeService _centroCustoCausaNaoConformidadeService;

    private readonly IImplementacaoEvitarReincidenciaNaoConformidadeService
        _implementacaoEvitarReincidenciaNaoConformidadeService;

    private readonly IOrdemRetrabalhoNaoConformidadeService _ordemRetrabalhoNaoConformidadeService;
    private readonly IOperacaoRetrabalhoNaoConformidadeService _operacaoRetrabalhoNaoConformidadeService;
    private readonly ICompanyStore _companyStore;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";

    public RelatoriosProvider(IReportingApi externalReportingService,
        IDateTimeProvider dateTimeProvider, ICurrentUser currentUser, IApiClientCallBuilder apiClientCallBuilder,
        ILogger<RelatoriosProvider> logger, IUserStore userStore, INaoConformidadeProvider naoConformidadeProvider,
        ICausaNaoConformidadeProvider causaNaoConformidadeProvider,
        ISolucaoNaoConformidadeProvider solucaoNaoConformidadeProvider,
        IDefeitoNaoConformidadeProvider defeitoNaoConformidadeProvider,
        IAcaoPreventivaNaoConformidadeProvider acaoPreventivaNaoConformidadeProvider,
        ICentroCustoCausaNaoConformidadeService centroCustoCausaNaoConformidadeService,
        IImplementacaoEvitarReincidenciaNaoConformidadeService implementacaoEvitarReincidenciaNaoConformidadeService,
        IOrdemRetrabalhoNaoConformidadeService ordemRetrabalhoNaoConformidadeService,
        IOperacaoRetrabalhoNaoConformidadeService operacaoRetrabalhoNaoConformidadeService,
        ICompanyStore companyStore)
    {
        _externalReportingService = externalReportingService;
        _dateTimeProvider = dateTimeProvider;
        _currentUser = currentUser;
        _apiClientCallBuilder = apiClientCallBuilder;
        _logger = logger;
        _userStore = userStore;
        _naoConformidadeProvider = naoConformidadeProvider;
        _causaNaoConformidadeProvider = causaNaoConformidadeProvider;
        _solucaoNaoConformidadeProvider = solucaoNaoConformidadeProvider;
        _defeitoNaoConformidadeProvider = defeitoNaoConformidadeProvider;
        _acaoPreventivaNaoConformidadeProvider = acaoPreventivaNaoConformidadeProvider;
        _centroCustoCausaNaoConformidadeService = centroCustoCausaNaoConformidadeService;
        _implementacaoEvitarReincidenciaNaoConformidadeService = implementacaoEvitarReincidenciaNaoConformidadeService;
        _ordemRetrabalhoNaoConformidadeService = ordemRetrabalhoNaoConformidadeService;
        _operacaoRetrabalhoNaoConformidadeService = operacaoRetrabalhoNaoConformidadeService;
        _companyStore = companyStore;
    }

    public async Task<ExportarRelatorioNaoConformidadeOutput> ExportarRelatorio(Guid idNaoConformidade)
    {
        var agregacao = await GetAgregacaoParaRelatorio(idNaoConformidade);
        var readModels = await GetReadModelsParaRelatorio(agregacao);
        var userPreferences = await _userStore.GetUserPreferencesAsync(_currentUser.Id);
        var empresa = await _companyStore.GetCompanyDetailsAsync(agregacao.NaoConformidade.CompanyId.ToString());

        var relatorioNaoConformidadeBuilder =
            new RelatorioNaoConformidadeBuilder(_dateTimeProvider, agregacao, userPreferences, readModels, empresa);

        var relatorioInput = relatorioNaoConformidadeBuilder
            .WithNaoConformidade()
            .WithCausasNaoConformidade()
            .WithDefeitosNaoConformidade()
            .WithSolucoesNaoConformidade()
            .WithAcoesPreventivasNaoConformidade()
            .WithCentroCustoCausaNaoConformidade()
            .WithImplementacaoEvitarReincidenciaNaoConformidade()
            .WithOrdemRetrabalhoNaoConformidade()
            .WithOperacaoRetrabalhoNaoConformidade()
            .Build();

        try
        {
            var exportInput = new ApiReportingExportInput()
            {
                ReportId = RelatorioPadraoConsts.ReportId,
                ReportingOutputType = ApiReportingOutputType.Pdf,
                Data = relatorioInput,
            };
            
            var response = await _externalReportingService.ExportAsync(exportInput);

            await using (var stream = await response.HttpResponseMessage.Content.ReadAsStreamAsync())
            {
                var output = new ExportarRelatorioNaoConformidadeOutput
                {
                    FileBytes = stream.ReadAllBytes(),
                    Success = true
                };
                return output;
            }
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            _logger.Log(LogLevel.Error, e.StackTrace);

            var output = new ExportarRelatorioNaoConformidadeOutput
            {
                Success = false,
                Message = "Ocorreu um erro na impressão da NC, favor entrar em contato com o administrador."
            };

            return output;
        }
    }


    private async Task<AgregacaoNaoConformidadeOutput> GetAgregacaoParaRelatorio(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}/agregacao")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var agregacao = await callBuilder.ResponseCallAsync<AgregacaoNaoConformidadeOutput>();
        return agregacao;
    }

    private async Task<GroupedReadModels> GetReadModelsParaRelatorio(AgregacaoNaoConformidadeOutput agregacao)
    {
        var idNaoConformidade = agregacao.NaoConformidade.Id;

        var flagedInput = new GetListWithDefeitoIdFlagInput();
        var naoConformidadeView = await _naoConformidadeProvider.GetView(agregacao.NaoConformidade.Id);

        var defeitoNaoConformidadeViews = await _defeitoNaoConformidadeProvider.GetList(null, idNaoConformidade);
        var causaNaoConformidadeViews = await _causaNaoConformidadeProvider
            .GetList(flagedInput, idNaoConformidade, Guid.NewGuid(), false);
        var solucaoNaoConformidadeViews =
            await _solucaoNaoConformidadeProvider.GetList(flagedInput, idNaoConformidade, Guid.NewGuid(), false);
        var acaoPreventivaNaoConformidadeViews =
            await _acaoPreventivaNaoConformidadeProvider.GetList(idNaoConformidade, Guid.NewGuid(), flagedInput, false);
        var implementacoesEficaciaEvitarReincidencia =
            await _implementacaoEvitarReincidenciaNaoConformidadeService.GetListView(idNaoConformidade,
                new GetListViewInput
                {
                    MaxResultCount = 100
                });
        var ordemRetrabalhoNaoConformidade = await _ordemRetrabalhoNaoConformidadeService.GetView(idNaoConformidade);

        var operacaoRetrabalhoNaoConformidade = await _operacaoRetrabalhoNaoConformidadeService.Get(idNaoConformidade);

        var operacoesOperacaoRetrabalhoNaoConformidade = new List<OperacaoViewOutput>();

        if (operacaoRetrabalhoNaoConformidade != null)
        {
            var operacoesOperacaoRetrabalhoNaoConformidadePaginadas = await _operacaoRetrabalhoNaoConformidadeService
                .GetOperacoesView(idNaoConformidade, operacaoRetrabalhoNaoConformidade.Id,
                    new PagedFilteredAndSortedRequestInput
                    {
                        MaxResultCount = 100
                    });

            operacoesOperacaoRetrabalhoNaoConformidade = operacoesOperacaoRetrabalhoNaoConformidadePaginadas.Items;
        }

        var centroCustos = await _centroCustoCausaNaoConformidadeService
            .GetListView(idNaoConformidade, new PagedFilteredAndSortedRequestInput
            {
                MaxResultCount = 100
            });

        var output = new GroupedReadModels
        {
            NaoConformidadeViewOutput = naoConformidadeView,
            CausaNaoConformidadeViewOutput = causaNaoConformidadeViews.Items,
            DefeitoNaoConformidadeViewOutput = defeitoNaoConformidadeViews.Items,
            SolucaoNaoConformidadeViewOutput = solucaoNaoConformidadeViews.Items,
            AcaoPreventivaNaoConformidadeViewOutput = acaoPreventivaNaoConformidadeViews.Items,
            CentroCustoCausaNaoConformidadeViewOutputs = centroCustos.Items,
            ImplementacaoEvitarReincidenciaNaoConformidadeViewOutputs = implementacoesEficaciaEvitarReincidencia.Items,
            OrdemRetrabalhoNaoConformidadeViewOutput = ordemRetrabalhoNaoConformidade,
            OperacaoRetrabalhoNaoConformidade = operacaoRetrabalhoNaoConformidade,
            OperacoesOperacaoRetrabalhoNaoConformidade = operacoesOperacaoRetrabalhoNaoConformidade
        };

        return output;
    }
}
