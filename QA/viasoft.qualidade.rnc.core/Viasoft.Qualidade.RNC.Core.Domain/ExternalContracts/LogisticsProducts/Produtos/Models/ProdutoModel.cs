using System;
using Newtonsoft.Json;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsProducts.Produtos.Models;

public class ProdutoModel: IProdutoModel
{
    public Guid Id { get; set; }
    [JsonProperty("Code")]
    public string Codigo { get; set; }
    [JsonProperty("Description")]
    public string Descricao { get; set; }
   
    public Guid IdUnidade { get; set; }
    public Guid? IdCategoria { get; set; }
}