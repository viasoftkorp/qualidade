using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

public class ExternalMovimentarEstoqueListaInput
{
    [JsonPropertyName("Itens")]
    public List<ExternalMovimentarEstoqueItemInput> Itens { get; set; }
}