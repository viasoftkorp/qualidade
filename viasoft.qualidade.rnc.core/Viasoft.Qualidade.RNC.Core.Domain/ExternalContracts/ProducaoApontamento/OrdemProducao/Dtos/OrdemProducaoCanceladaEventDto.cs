using Newtonsoft.Json;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Dtos
{
    public class OrdemProducaoCanceladaEventDto
    {
        public int NumeroOdf { get; set; }
        [JsonProperty("OrdemRetrabalho")]
        public bool IsOrdemRetrabalho { get; set; }
    }
}