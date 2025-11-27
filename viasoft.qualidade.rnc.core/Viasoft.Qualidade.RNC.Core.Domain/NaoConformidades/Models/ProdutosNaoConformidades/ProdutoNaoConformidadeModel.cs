using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ProdutosNaoConformidades;

public class ProdutoNaoConformidadeModel : IProdutoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdProduto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public string Detalhamento { get; set; }
    public decimal Quantidade { get; set; }
    public string OperacaoEngenharia { get; set; }
    public Guid CompanyId { get; set; }

    public ProdutoNaoConformidadeModel()
    {
    }

    public ProdutoNaoConformidadeModel(IProdutoNaoConformidadeModel model)
    {
        Id = model.Id;
        IdProduto = model.IdProduto;
        IdNaoConformidade = model.IdNaoConformidade;
        Quantidade = model.Quantidade;
        Detalhamento = model.Detalhamento;
        OperacaoEngenharia = model.OperacaoEngenharia;
        CompanyId = model.CompanyId;
    }
}
