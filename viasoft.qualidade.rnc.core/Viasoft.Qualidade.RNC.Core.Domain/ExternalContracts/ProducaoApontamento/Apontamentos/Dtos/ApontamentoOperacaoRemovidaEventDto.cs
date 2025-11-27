using Newtonsoft.Json;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;

public class ApontamentoOperacaoRemovidaEventDto
{
    public string NumeroOperacao { get; set; }
    public int NumeroOdf { get; set; }
    [JsonProperty("OperacaoRetrabalho")]
    public bool IsOperacaoRetrabalho { get; set; }
}