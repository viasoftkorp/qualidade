using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsProducts.Produtos.Models;

public interface IProdutoModel
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public Guid IdUnidade { get; set; }
    public Guid? IdCategoria { get; set; }

}