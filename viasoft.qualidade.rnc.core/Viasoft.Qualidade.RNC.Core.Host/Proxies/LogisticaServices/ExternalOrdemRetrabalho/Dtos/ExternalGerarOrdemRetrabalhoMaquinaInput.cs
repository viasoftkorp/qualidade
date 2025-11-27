using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;

public class ExternalGerarOrdemRetrabalhoMaquinaInput
{
    [JsonPropertyName("IdMaquina")]
    public int IdMaquina { get; set; }
    
    [JsonPropertyName("QuantidadeHoras")]
    public decimal QuantidadeHoras { get; set; }
    
    [JsonPropertyName("Operacao")]
    public string Operacao { get; set; }
    
    [JsonPropertyName("Sequencia")]
    public string Sequencia { get; set; }
    
    [JsonPropertyName("DescricaoOperacao")]
    public string DescricaoOperacao { get; set; }
    
    [JsonPropertyName("ProdutividadeMaquina")]
    public decimal ProdutividadeMaquina { get; set; }

    [JsonPropertyName("ApontarMaquina")]
    public bool ApontarMaquina { get; set; }

    [JsonPropertyName("Regula")]
    public decimal Regula { get; set; }

    [JsonPropertyName("Peca")]
    public string Peca { get; set; }

    [JsonPropertyName("PecaPai")]
    public string PecaPai { get; set; }

    [JsonPropertyName("Nivel")]
    public int Nivel { get; set; }

    [JsonPropertyName("Posicao")]
    public string Posicao { get; set; }

    [JsonPropertyName("ConfirmarMateriais")]
    public string ConfirmarMateriais { get; set; }

    [JsonPropertyName("ControlarApontamento")]
    public string ControlarApontamento { get; set; }
}