using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ProdutosNaoConformidades;

public interface IProdutoNaoConformidadeModel
{
    Guid Id { get; }
    Guid IdProduto { get; }
    string Detalhamento { get; }
    Guid IdNaoConformidade { get; }
    decimal Quantidade { get; }
    string OperacaoEngenharia { get; }
    Guid CompanyId { get; }
}