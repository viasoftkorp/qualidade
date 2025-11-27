using Newtonsoft.Json;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;

public class ApontamentoInicioProducaoEventDto
{
    [JsonProperty("Odf")]
    public int NumeroOdf { get; set; }
    
    [JsonProperty("Operacao")]
    public string NumeroOperacao { get; set; }
    
    [JsonProperty("OperacaoRetrabalho")]
    public bool IsOperacaoRetrabalho { get; set; }
    
    [JsonProperty("OrdemRetrabalho")]
    public bool IsOrdemRetrabalho { get; set; }

}