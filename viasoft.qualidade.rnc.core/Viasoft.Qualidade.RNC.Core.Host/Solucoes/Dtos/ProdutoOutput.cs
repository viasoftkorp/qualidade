using System;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsProducts.Produtos.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class ProdutoOutput : IProdutoModel
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public Guid IdUnidade { get; set; }

    public Guid? IdCategoria { get; set; }
}