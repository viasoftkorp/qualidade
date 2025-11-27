using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

public class ExternalMovimentarEstoqueLoteInput
{
    [JsonPropertyName("PickingItemLoteId")]
    public string PickingItemLoteId { get; set; }
    
    [JsonPropertyName("IdEstoqueLocal")]
    public int? IdEstoqueLocal { get; set; }

    [JsonPropertyName("CodigoLocalDestino")]
    public int CodigoLocalDestino { get; set; }

    [JsonPropertyName("CodigoLocalOrigem")]
    public int CodigoLocalOrigem { get; set; }

    [JsonPropertyName("Documento")]
    public string Documento { get; set; }

    [JsonPropertyName("OdfOrigem")]
    public int OdfOrigem { get; set; }

    [JsonPropertyName("OdfDestino")]
    public int OdfDestino { get; set; }

    [JsonPropertyName("LoteOrigem")]
    public string LoteOrigem { get; set; }

    [JsonPropertyName("LoteDestino")]
    public string LoteDestino { get; set; }

    [JsonPropertyName("PesoLiquido")]
    public decimal PesoLiquido { get; set; }

    [JsonPropertyName("PesoBruto")]
    public decimal PesoBruto { get; set; }

    [JsonPropertyName("DataValidade")]
    public string DataValidade { get; set; }

    [JsonPropertyName("DataFabricacao")]
    public string DataFabricacao { get; set; }

    [JsonPropertyName("CodigoProduto")]
    public string CodigoProduto { get; set; }

    [JsonPropertyName("PedidoVendaOrigem")]
    public string PedidoVendaOrigem { get; set; }

    [JsonPropertyName("PedidoVendaDestino")]
    public string PedidoVendaDestino { get; set; }

    [JsonPropertyName("Quantidade")]
    public decimal Quantidade { get; set; }

    [JsonPropertyName("CodigoArmazemDestino")]
    public string CodigoArmazemDestino { get; set; }

    [JsonPropertyName("CodigoArmazemOrigem")]
    public string CodigoArmazemOrigem { get; set; }

    [JsonPropertyName("CodigoDeBarras")]
    public string CodigoDeBarras { get; set; }

    [JsonPropertyName("IdEmpresa")]
    public int IdEmpresa { get; set; }
    [JsonPropertyName("TransferindoParaLocalRetrabalho")]

    public bool TransferindoParaLocalRetrabalho { get; set; }

    [JsonPropertyName("Pacotes")] public List<object> Pacotes { get; set; } = new List<object>();

    [JsonPropertyName("Series")] public List<object> Series { get; set; } = new List<object>();

}