using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class ProdutoSolucaoOutput : ProdutoSolucaoModel
{
    public ProdutoSolucaoOutput(ProdutoSolucao produtoSolucao)
    {
        Id = produtoSolucao.Id;
        IdSolucao = produtoSolucao.IdSolucao;
        IdProduto = produtoSolucao.IdProduto;
        Quantidade = produtoSolucao.Quantidade;
        OperacaoEngenharia = produtoSolucao.OperacaoEngenharia;
    }
}