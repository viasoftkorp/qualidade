using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

public class ProdutoNaoConformidadeInput
{
    public Guid Id { get; set; }
    public Guid IdProduto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    public string Detalhamento { get; set; }
    public string OperacaoEngenharia { get; set; }
}