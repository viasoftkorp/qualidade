using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class SolucaoOutput : SolucaoModel
{
    public SolucaoOutput(Solucao solucao)
    {
        Id = solucao.Id;
        Descricao = solucao.Descricao;
        Codigo = solucao.Codigo;
        Detalhamento = solucao.Detalhamento;
        Imediata = solucao.Imediata;
        IsAtivo = solucao.IsAtivo;
    }

    public SolucaoOutput()
    {
    }
}