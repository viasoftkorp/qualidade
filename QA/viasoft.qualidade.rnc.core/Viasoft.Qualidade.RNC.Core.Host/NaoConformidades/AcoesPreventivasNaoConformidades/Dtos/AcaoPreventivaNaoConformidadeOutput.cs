using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.AcoesPreventivasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

public class AcaoPreventivaNaoConformidadeOutput: AcaoPreventivaNaoConformidadeModel
{
    public AcaoPreventivaNaoConformidadeOutput(AcaoPreventivaNaoConformidade acao)
    {
        Id = acao.Id;
        IdNaoConformidade = acao.IdNaoConformidade;
        IdDefeitoNaoConformidade = acao.IdDefeitoNaoConformidade;
        IdAcaoPreventiva = acao.IdAcaoPreventiva;
        Acao = acao.Acao;
        Detalhamento = acao.Detalhamento;
        Implementada = acao.Implementada;
        DataAnalise = acao.DataAnalise;
        DataVerificacao = acao.DataVerificacao;
        IdAuditor = acao.IdAuditor;
        IdResponsavel = acao.IdResponsavel;
        NovaData = acao.NovaData;
        DataPrevistaImplantacao = acao.DataPrevistaImplantacao;
    }
}