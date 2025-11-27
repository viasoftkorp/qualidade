using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;

public class GroupedReadModels
{
    public NaoConformidadeViewOutput NaoConformidadeViewOutput { get; set; }
    public List<CausaNaoConformidadeViewOutput> CausaNaoConformidadeViewOutput { get; set; }
    public List<SolucaoNaoConformidadeViewOutput> SolucaoNaoConformidadeViewOutput { get; set; }
    public List<DefeitoNaoConformidadeViewOutput> DefeitoNaoConformidadeViewOutput { get; set; }
    public List<AcaoPreventivaNaoConformidadeViewOutput> AcaoPreventivaNaoConformidadeViewOutput { get; set; }
    public List<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput> ImplementacaoEvitarReincidenciaNaoConformidadeViewOutputs { get; set; }
    public List<CentroCustoCausaNaoConformidadeViewOutput> CentroCustoCausaNaoConformidadeViewOutputs { get; set; }
    public OrdemRetrabalhoNaoConformidadeViewOutput OrdemRetrabalhoNaoConformidadeViewOutput { get; set; }
    public OperacaoRetrabalhoNaoConformidade OperacaoRetrabalhoNaoConformidade { get; set; }
    public List<OperacaoViewOutput> OperacoesOperacaoRetrabalhoNaoConformidade { get; set; }
}
