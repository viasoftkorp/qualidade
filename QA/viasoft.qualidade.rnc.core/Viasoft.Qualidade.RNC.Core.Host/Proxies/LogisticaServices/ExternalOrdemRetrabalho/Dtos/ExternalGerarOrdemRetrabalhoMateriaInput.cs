using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;

public class ExternalGerarOrdemRetrabalhoMaterialInput
{

    [JsonPropertyName("Quantidade")]
    public decimal Quantidade { get; set; }
    
    [JsonPropertyName("CodigoProduto")]
    public string CodigoProduto { get; set; }
    
    [JsonPropertyName("Operacao")]
    public string Operacao { get; set; }
    
    [JsonPropertyName("Categoria")]
    public string Categoria { get; set; }
    
    [JsonPropertyName("Sequencia")]
    public string Sequencia { get; set; }
    
    [JsonPropertyName("Revisao")]
    public string Revisao { get; set; }

    [JsonPropertyName("IdRoteiro")]
    public int IdRoteiro { get; set; }

    [JsonPropertyName("Peca")]
    public string Peca { get; set; }

    [JsonPropertyName("PecaPai")]
    public string PecaPai { get; set; }

    [JsonPropertyName("Nivel")]
    public int Nivel { get; set; }

    [JsonPropertyName("Posicao")]
    public string Posicao { get; set; }

    [JsonPropertyName("Expedicao")]
    public string Expedicao { get; set; }

    [JsonPropertyName("NaoGerarCusto")]
    public string NaoGerarCusto { get; set; }

    [JsonPropertyName("Reaproveitado")]
    public string Reaproveitado { get; set; }

    [JsonPropertyName("AjustarQtdApontamento")]
    public string AjustarQtdApontamento { get; set; }

    [JsonPropertyName("LocalConsumo")]
    public int LocalConsumo { get; set; }
}