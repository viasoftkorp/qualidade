using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;

public class ExportarRelatorioNaoConformidadeInput
{
    public NaoConformidadeDataSource NaoConformidade { get; set; }
    
    #nullable enable
    public AcaoPreventivaDataSource? AcoesPreventivasNaoConformidade { get; set; }
    public SolucaoDataSource? SolucoesNaoConformidade { get; set; }
    public DefeitoDataSource? DefeitosNaoConformidade { get; set; }
    public CausaDataSource? CausasNaoConformidade { get; set; }
    public ImplementacaoEvitarReincidenciaDataSource? ImplementacaoEvitarReincidenciaNaoConformidade { get; set; }
    public CentroCustoCausaNaoConformidadeDataSource? CentroCustoCausaNaoConformidade { get; set; }
    public OrdemRetrabalhoNaoConformidadeDataSource? OrdemRetrabalhoNaoConformidade { get; set; }
    public OperacaoRetrabalhoNaoConformidadeDataSource? OperacaoRetrabalhoNaoConformidade { get; set; }
    public OperacoesOperacaoRetrabalhoNaoConformidadeDataSource? OperacoesOperacaoRetrabalhoNaoConformidade { get; set; }
}
