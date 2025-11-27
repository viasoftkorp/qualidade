using Newtonsoft.Json;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events
{
    [Endpoint("Korp.Producao.Apontamento.FimProducao")]
    public class ApontamentoFimProducaoEvent : IEvent
    {
        [JsonProperty("ProducaoFinalizadaEvent")]
        public ApontamentoFimProducaoEventDto ApontamentoProducaoEventDto { get; set; }

        public ApontamentoFimProducaoEvent()
        {
        }

        public ApontamentoFimProducaoEvent(ApontamentoFimProducaoEventDto apontamentoProducaoEventDto)
        {
            ApontamentoProducaoEventDto = apontamentoProducaoEventDto;
        }
    }
}