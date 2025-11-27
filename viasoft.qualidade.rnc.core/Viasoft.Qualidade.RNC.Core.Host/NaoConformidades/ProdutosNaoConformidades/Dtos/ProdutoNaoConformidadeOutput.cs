using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ProdutosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

public class ProdutoNaoConformidadeOutput : ProdutoNaoConformidadeModel
{
    public ProdutoNaoConformidadeOutput(ProdutoNaoConformidade produto)
    {
        Id = produto.Id;
        IdProduto = produto.IdProduto;
        IdNaoConformidade = produto.IdNaoConformidade;
        Quantidade = produto.Quantidade;
        Detalhamento = produto.Detalhamento;
        OperacaoEngenharia = produto.OperacaoEngenharia;
    }
}