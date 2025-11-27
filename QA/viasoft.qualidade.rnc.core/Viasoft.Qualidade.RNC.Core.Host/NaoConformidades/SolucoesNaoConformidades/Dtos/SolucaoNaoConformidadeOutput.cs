using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.SolucoesNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

public class SolucaoNaoConformidadeOutput : SolucaoNaoConformidadeModel

{
    public SolucaoNaoConformidadeOutput(SolucaoNaoConformidade solucao)
    {
        Id = solucao.Id;
        IdNaoConformidade = solucao.IdNaoConformidade;
        IdDefeitoNaoConformidade = solucao.IdDefeitoNaoConformidade;
        SolucaoImediata = solucao.SolucaoImediata;
        DataAnalise = solucao.DataAnalise;
        DataPrevistaImplantacao = solucao.DataPrevistaImplantacao;
        IdResponsavel = solucao.IdResponsavel;
        CustoEstimado = solucao.CustoEstimado;
        NovaData = solucao.NovaData;
        DataVerificacao = solucao.DataVerificacao;
        IdAuditor = solucao.IdAuditor;
        Detalhamento = solucao.Detalhamento;
        IdSolucao = solucao.IdSolucao;
    }
}