using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;

public class ExternalEstornarOrdemRetrabalhoInput
{
    [JsonPropertyName("Odf")]
    public int Odf { get; set; }
    
    [JsonPropertyName("OdfVenda")]
    public int OdfVenda { get; set; }
    
    [JsonPropertyName("Quantidade")]
    public decimal Quantidade { get; set; }
    
    [JsonPropertyName("SaldoOdf")]
    public decimal SaldoOdf { get; set; }
    
    [JsonPropertyName("Motivo")]
    public string Motivo { get; set; }
    
    [JsonPropertyName("Situacao")]
    public string Situacao { get; set; }
    
    [JsonPropertyName("CodigoProduto")]
    public string CodigoProduto { get; set; }
    
    [JsonPropertyName("PedidoVenda")]
    public string PedidoVenda { get; set; }
}