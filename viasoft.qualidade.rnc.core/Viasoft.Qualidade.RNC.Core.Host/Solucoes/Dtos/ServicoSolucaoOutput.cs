using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class ServicoSolucaoOutput : ServicoSolucaoModel
{
    public ServicoSolucaoOutput(ServicoSolucao servicoSolucao)
    {
        Id = servicoSolucao.Id;
        IdSolucao = servicoSolucao.IdSolucao;
        IdProduto = servicoSolucao.IdProduto;
        Quantidade = servicoSolucao.Quantidade;
        Horas = servicoSolucao.Horas;
        Minutos = servicoSolucao.Minutos;
        IdRecurso = servicoSolucao.IdRecurso;
        OperacaoEngenharia = servicoSolucao.OperacaoEngenharia;
    }
}