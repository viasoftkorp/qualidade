using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

public class ExternalMovimentarEstoqueItemInput
{
    [JsonPropertyName("Lotes")]
    public List<ExternalMovimentarEstoqueLoteInput> Lotes { get; set; }
    [JsonPropertyName("IdPickingItem")]
    public string IdPickingItem { get; set; }
}