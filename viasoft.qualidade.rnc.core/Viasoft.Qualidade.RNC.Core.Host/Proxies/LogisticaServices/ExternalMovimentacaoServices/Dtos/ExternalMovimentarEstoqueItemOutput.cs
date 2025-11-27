using System.Collections.Generic;
using System.Text.Json.Serialization;
using Viasoft.Qualidade.RNC.Core.Domain.Legacy;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

public class ExternalMovimentarEstoqueItemOutput
{ 
    public List<ExternalMovimentarEstoqueItemOutputResultado> Resultado {get;set;}
    public KorpErro Error { get; set; }
}

public class ExternalMovimentarEstoqueItemOutputResultado
{
    public string IdPickingItem { get; set; }
    public List<ExternalMovimentarEstoqueItemResultado> Resultados { get; set; }
}

public class ExternalMovimentarEstoqueItemResultado
{
    public int CodigoLocalDestino { get; set; }
    [JsonPropertyName("IdEstoqueLocalDestino")]
    public int LegacyIdEstoqueLocalDestino { get; set; }
}