using Newtonsoft.Json;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events
{
    [Endpoint("Korp.Producao.Apontamento.InicioProducao")]
    public class ApontamentoInicioProducaoEvent : IEvent
    {
        [JsonProperty("ProducaoIniciada")]
        public ApontamentoInicioProducaoEventDto ApontamentoProducaoEventDto { get; set; }
        
        public ApontamentoInicioProducaoEvent()
        {
        }

        public ApontamentoInicioProducaoEvent(ApontamentoInicioProducaoEventDto apontamentoProducaoEventDtoEvent)
        {
            ApontamentoProducaoEventDto = apontamentoProducaoEventDtoEvent;
        }
    }
}