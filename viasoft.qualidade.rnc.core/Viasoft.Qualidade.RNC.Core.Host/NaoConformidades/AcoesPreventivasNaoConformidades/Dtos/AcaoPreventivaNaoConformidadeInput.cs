using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.AcoesPreventivasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

public class AcaoPreventivaNaoConformidadeInput : AcaoPreventivaNaoConformidadeModel
{
    public AcaoPreventivaNaoConformidadeInput()
    {
    }

    public AcaoPreventivaNaoConformidadeInput(AcaoPreventivaNaoConformidade acao)
    {
        Id = acao.Id;
        IdNaoConformidade = acao.IdNaoConformidade;
        IdAcaoPreventiva = acao.IdAcaoPreventiva;
        IdDefeitoNaoConformidade = acao.IdDefeitoNaoConformidade;
        Acao = acao.Acao;
        Detalhamento = acao.Detalhamento;
        IdResponsavel = acao.IdResponsavel;
        DataAnalise = acao.DataAnalise;
        DataPrevistaImplantacao = acao.DataPrevistaImplantacao;
        IdAuditor = acao.IdAuditor;
        Implementada = acao.Implementada;
        DataVerificacao = acao.DataVerificacao;
        NovaData = acao.NovaData;
    }
}