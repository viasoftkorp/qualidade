using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

public class ProdutoNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdProduto { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public string UnidadeMedida { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    [IsArrayOfBytes]
    public string OperacaoEngenharia { get; set; }

    public ProdutoNaoConformidadeViewOutput()
    {
    }
    public ProdutoNaoConformidadeViewOutput(ProdutoNaoConformidade produtoNaoConformidade, Produto produto, UnidadeMedidaProduto unidadeMedidaProduto)
    {
        Id = produtoNaoConformidade.Id;
        IdProduto = produtoNaoConformidade.IdProduto;
        Codigo = produto.Codigo;
        Descricao = produto.Descricao;
        Detalhamento = produtoNaoConformidade.Detalhamento;
        UnidadeMedida = unidadeMedidaProduto.Descricao;
        IdNaoConformidade = produtoNaoConformidade.IdNaoConformidade;
        Quantidade = produtoNaoConformidade.Quantidade;
        OperacaoEngenharia = produtoNaoConformidade.OperacaoEngenharia;
    }
}