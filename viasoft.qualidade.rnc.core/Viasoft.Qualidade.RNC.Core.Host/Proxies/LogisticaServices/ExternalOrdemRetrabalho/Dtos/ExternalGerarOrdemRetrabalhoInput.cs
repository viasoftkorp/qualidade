using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;

public class ExternalGerarOrdemRetrabalhoInput
{  
    [JsonPropertyName("Quantidade")]
    public decimal Quantidade { get; set; }
    
    [JsonPropertyName("IdEmpresa")]
    public int IdEmpresa { get; set; }
    
    [JsonPropertyName("CodigoProduto")]
    public string CodigoProduto { get; set; }

    [JsonPropertyName("CodigoCliente")]
    public string CodigoCliente { get; set; }
    
    [JsonPropertyName("DataEntrega")]
    public string DataEntrega { get; set; }
    
    [JsonPropertyName("Pedido")]
    public string Pedido { get; set; }

    [JsonPropertyName("OdfOrigem")]
    public int OdfOrigem { get; set; }
    
    [JsonPropertyName("Servico")]
    public bool Servico { get; set; }
    
    [JsonPropertyName("Projetar")]
    public bool Projetar { get; set; }
    
    [JsonPropertyName("Retrabalho")]
    public bool Retrabalho { get; set; }
    
    [JsonPropertyName("AnalisarReversa")]
    public bool AnalisarReversa { get; set; }
    
    [JsonPropertyName("Lote")]
    public string Lote { get; set; }
    
    [JsonPropertyName("LocalDestino")]
    public int LocalDestino { get; set; }
    
    [JsonPropertyName("Materias")]
    public List<ExternalGerarOrdemRetrabalhoMaterialInput> Materias { get; set; }
    
    [JsonPropertyName("Maquinas")]
    public List<ExternalGerarOrdemRetrabalhoMaquinaInput> Maquinas { get; set; }
}