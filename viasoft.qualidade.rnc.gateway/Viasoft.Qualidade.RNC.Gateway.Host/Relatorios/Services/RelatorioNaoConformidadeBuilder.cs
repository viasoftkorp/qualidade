using System.Collections.Generic;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.Identity.Abstractions.Model;
using Viasoft.Core.MultiTenancy.Abstractions.Company.Model;
using Viasoft.Qualidade.RNC.Gateway.Host.Extensions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Services;

public class RelatorioNaoConformidadeBuilder
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AgregacaoNaoConformidadeOutput _agregacaoNaoConformidadeOutput;
    private readonly UserPreferences _userPreferences;
    private readonly GroupedReadModels _groupedReadModels;
    private readonly ExportarRelatorioNaoConformidadeInput _relatorio;
    private readonly CompanyDetails _empresa;

    public RelatorioNaoConformidadeBuilder(IDateTimeProvider dateTimeProvider, 
        AgregacaoNaoConformidadeOutput agregacaoNaoConformidadeOutput,
        UserPreferences userPreferences,
        GroupedReadModels groupedReadModels,
        CompanyDetails empresa)
    {
        _dateTimeProvider = dateTimeProvider;
        _agregacaoNaoConformidadeOutput = agregacaoNaoConformidadeOutput;
        _userPreferences = userPreferences;
        _groupedReadModels = groupedReadModels;
        _empresa = empresa;

        _relatorio = new ExportarRelatorioNaoConformidadeInput();
    }

    public RelatorioNaoConformidadeBuilder WithNaoConformidade()
    {
        var agregacao = _agregacaoNaoConformidadeOutput;
        var naoConformidadeView = _groupedReadModels.NaoConformidadeViewOutput;
        var empresa = _empresa;
        
        _relatorio.NaoConformidade = new NaoConformidadeDataSource
        {
            NaoConformidade = new List<RelatorioNaoConformidade>
            {
                new RelatorioNaoConformidade
                {
                    CodigoRnc = agregacao.NaoConformidade.Codigo.ToString(),
                    Origem = agregacao.NaoConformidade.Origem.ToString(),
                    Data = _dateTimeProvider.UtcNow().ToLocal(_userPreferences.DefaultUserTimeZone).ToString(),
                    NotaFiscal = agregacao.NaoConformidade.NumeroNotaFiscal,
                    Cliente = naoConformidadeView.Cliente,
                    Fornecedor = naoConformidadeView.Fornecedor,
                    Produto = naoConformidadeView.Produto,
                    Lote = agregacao.NaoConformidade.NumeroLote,
                    CodigoCliente = naoConformidadeView.CodigoCliente,
                    CodigoFornecedor = naoConformidadeView.CodigoFornecedor,
                    CodigoInterno = naoConformidadeView.CodigoProduto,
                    Revisao = agregacao.NaoConformidade.Revisao,
                    LoteTotal = agregacao.NaoConformidade.LoteTotal,
                    LoteParcial = agregacao.NaoConformidade.LoteParcial,
                    Rejeitado = agregacao.NaoConformidade.Rejeitado,
                    AceitoConcessao = agregacao.NaoConformidade.AceitoConcessao,
                    RetrabalhadoPeloCliente = agregacao.NaoConformidade.RetrabalhoPeloCliente,
                    RetrabalhadoNoCliente = agregacao.NaoConformidade.RetrabalhoNoCliente,
                    TimeEnvolvido = agregacao.NaoConformidade.Equipe,
                    NaoConformidadePotencial = agregacao.NaoConformidade.NaoConformidadeEmPotencial,
                    NaoConformidade = agregacao.NaoConformidade.RelatoNaoConformidade,
                    MelhoriaPotencial = agregacao.NaoConformidade.MelhoriaEmPotencial,
                    DescricaoNaoConformidade = agregacao.NaoConformidade.Descricao,
                    ReclamacaoProcedente = agregacao.ReclamacaoNaoConformidade?.Procedentes.ToString(),
                    ReclamacaoImprocedente = agregacao.ReclamacaoNaoConformidade?.Improcedentes.ToString(),
                    QuantidadeLote = agregacao.ReclamacaoNaoConformidade?.QuantidadeLote.ToString(),
                    QuantidadeNaoConformidade = agregacao.ReclamacaoNaoConformidade?.QuantidadeNaoConformidade.ToString(),
                    AprovadoReclamacao = agregacao.ReclamacaoNaoConformidade?.DisposicaoProdutosAprovados.ToString(),
                    ConcessaoReclamacao = agregacao.ReclamacaoNaoConformidade?.DisposicaoProdutosConcessao.ToString(),
                    RejeitadoReclamacao = agregacao.ReclamacaoNaoConformidade?.Rejeitado.ToString(),
                    Retrabalho = agregacao.ReclamacaoNaoConformidade?.Retrabalho.ToString(),
                    RetrabalhoComOnus = agregacao.ReclamacaoNaoConformidade?.RetrabalhoComOnus ?? false,
                    RetrabalhoSemOnus = agregacao.ReclamacaoNaoConformidade?.RetrabalhoSemOnus ?? false,
                    DevolucaoFornecedor = agregacao.ReclamacaoNaoConformidade?.DevolucaoFornecedor ?? false,
                    Recodificar = agregacao.ReclamacaoNaoConformidade?.Recodificar ?? false,
                    Sucata = agregacao.ReclamacaoNaoConformidade?.Sucata ?? false,
                    Observacoes = agregacao.ReclamacaoNaoConformidade?.Observacao,
                    Reclamacoes = agregacao.ReclamacaoNaoConformidade != null,
                    Conclusoes = agregacao.ConclusaoNaoConformidade != null,
                    Evidencia = agregacao.ConclusaoNaoConformidade?.Evidencia,
                    Eficaz = agregacao.ConclusaoNaoConformidade?.Eficaz ?? false,
                    ClicloDeTempo = agregacao.ConclusaoNaoConformidade?.CicloDeTempo.ToString(),
                    NomeUsuarioCriador = naoConformidadeView.NomeUsuarioCriador,
                    SobrenomeUsuarioCriador = naoConformidadeView.SobrenomeUsuarioCriador,
                    DataCriacao = agregacao.NaoConformidade.DataCriacao.ToString(),
                    EmpresaId = empresa.Id.ToString(),
                    EmpresaLegacyId = empresa.LegacyCompanyId.ToString(),
                    EmpresaCnpj = empresa.Cnpj,
                    EmpresaRazaoSocial = empresa.CompanyName,
                    EmpresaFantasia = empresa.TradingName,
                }
            }
        };
        return this;
    }

    public RelatorioNaoConformidadeBuilder WithCausasNaoConformidade()
    {
        _relatorio.CausasNaoConformidade = new CausaDataSource
        {
            CausasNaoConformidade = _groupedReadModels.CausaNaoConformidadeViewOutput
        };
        return this;
    }
    public RelatorioNaoConformidadeBuilder WithDefeitosNaoConformidade()
    {
        _relatorio.DefeitosNaoConformidade = new DefeitoDataSource
        {
            DefeitosNaoConformidade = _groupedReadModels.DefeitoNaoConformidadeViewOutput
        };
        return this;
    }
    public RelatorioNaoConformidadeBuilder WithSolucoesNaoConformidade()
    {
        _relatorio.SolucoesNaoConformidade = new SolucaoDataSource
        {
            SolucoesNaoConformidade = _groupedReadModels.SolucaoNaoConformidadeViewOutput
                .ConvertAll(solucao => new RelatorioSolucaoNaoConformidade(solucao))
        };
        return this;
    }
    public RelatorioNaoConformidadeBuilder WithAcoesPreventivasNaoConformidade()
    {
        _relatorio.AcoesPreventivasNaoConformidade = new AcaoPreventivaDataSource
        {
            AcoesPreventivasNaoConformidade = _groupedReadModels.AcaoPreventivaNaoConformidadeViewOutput
                .ConvertAll(acaoPreventiva => new RelatorioAcaoPreventivaNaoConformidade(acaoPreventiva))
        };
        return this;
    }
    public RelatorioNaoConformidadeBuilder WithImplementacaoEvitarReincidenciaNaoConformidade()
    {
        _relatorio.ImplementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaDataSource
        {
            ImplementacaoEvitarReincidenciaNaoConformidade = _groupedReadModels.ImplementacaoEvitarReincidenciaNaoConformidadeViewOutputs
                .ConvertAll(implementacao => new RelatorioImplementacaoEvitarReincidenciaNaoConformidade(implementacao))
        };
        return this;
    }
    public RelatorioNaoConformidadeBuilder WithCentroCustoCausaNaoConformidade()
    {
        _relatorio.CentroCustoCausaNaoConformidade = new CentroCustoCausaNaoConformidadeDataSource
        {
            CentroCustoCausaNaoConformidades = _groupedReadModels.CentroCustoCausaNaoConformidadeViewOutputs
        };
        return this;
    }
    public RelatorioNaoConformidadeBuilder WithOrdemRetrabalhoNaoConformidade()
    {
        var ordemRetrabalhoNaoConformidadeViewOutput = _groupedReadModels.OrdemRetrabalhoNaoConformidadeViewOutput;

        if (ordemRetrabalhoNaoConformidadeViewOutput is null)
        {
            _relatorio.OrdemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidadeDataSource();
            return this;
        }
        
        _relatorio.OrdemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidadeDataSource
        {
            OrdemRetrabalhoNaoConformidade = new List<RelatorioOrdemRetrabalhoNaoConformidade>
            {
                new RelatorioOrdemRetrabalhoNaoConformidade
                {
                    IdNaoConformidade = ordemRetrabalhoNaoConformidadeViewOutput.IdNaoConformidade,
                    NumeroOdfRetrabalho = ordemRetrabalhoNaoConformidadeViewOutput.NumeroOdfRetrabalho,
                    Quantidade = ordemRetrabalhoNaoConformidadeViewOutput.Quantidade,
                    IdLocalOrigem = ordemRetrabalhoNaoConformidadeViewOutput.IdLocalOrigem,
                    DescricaoLocalOrigem = ordemRetrabalhoNaoConformidadeViewOutput.DescricaoLocalOrigem,
                    CodigoLocalOrigem = ordemRetrabalhoNaoConformidadeViewOutput.CodigoLocalOrigem,
                    IdEstoqueLocalDestino = ordemRetrabalhoNaoConformidadeViewOutput.IdEstoqueLocalDestino,
                    IdLocalDestino = ordemRetrabalhoNaoConformidadeViewOutput.IdLocalDestino,
                    DescricaoLocalDestino = ordemRetrabalhoNaoConformidadeViewOutput.DescricaoLocalDestino,
                    CodigoLocalDestino = ordemRetrabalhoNaoConformidadeViewOutput.CodigoLocalDestino,
                    CodigoArmazem = ordemRetrabalhoNaoConformidadeViewOutput.CodigoArmazem,
                    DataFabricacao = ordemRetrabalhoNaoConformidadeViewOutput.DataFabricacao.ToString(),
                    DataValidade = ordemRetrabalhoNaoConformidadeViewOutput.DataValidade.ToString(),
                    Status = ordemRetrabalhoNaoConformidadeViewOutput.Status.ToString()
                }
            } 
        };
        return this;
    }
    
    public RelatorioNaoConformidadeBuilder WithOperacaoRetrabalhoNaoConformidade()
    {
        _relatorio.OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeDataSource
        {
            OperacaoRetrabalhoNaoConformidade = new List<OperacaoRetrabalhoNaoConformidade>()
        };

        if (_groupedReadModels.OperacaoRetrabalhoNaoConformidade != null)
        {
            _relatorio.OperacaoRetrabalhoNaoConformidade.OperacaoRetrabalhoNaoConformidade.Add(_groupedReadModels.OperacaoRetrabalhoNaoConformidade);
        }
        
        var operacoesOperacaoRetrabalhoNaoConformidade = _groupedReadModels.OperacoesOperacaoRetrabalhoNaoConformidade;
        
        _relatorio.OperacoesOperacaoRetrabalhoNaoConformidade = new OperacoesOperacaoRetrabalhoNaoConformidadeDataSource
        {
            OperacoesOperacaoRetrabalhoNaoConformidade = operacoesOperacaoRetrabalhoNaoConformidade
                .ConvertAll(e => new RelatorioOperacoesOperacaoRetrabalhoNaoConformidade
                {
                    Id = e.Id,
                    NumeroOperacao = e.NumeroOperacao,
                    IdRecurso = e.IdRecurso,
                    DescricaoRecurso = e.DescricaoRecurso,
                    CodigoRecurso = e.CodigoRecurso,
                    IdOperacaoRetrabalhoNaoConformdiade = e.IdOperacaoRetrabalhoNaoConformdiade,
                    Status = e.Status.ToString()
                })
        };
        return this;
    }

    public ExportarRelatorioNaoConformidadeInput Build()
    {
        return _relatorio;
    }
}
