using Newtonsoft.Json;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events
{
    [Endpoint("Korp.Producao.Apontamento.Estorno")]
    public class ApontamentoEstornadoEvent : IEvent
    {
        [JsonProperty("ProducaoPedidoEvento")]
        public ApontamentoOperacaoEstornadaEventDto ApontamentoProducaoEventEventDto { get; set; }

        public ApontamentoEstornadoEvent()
        {
        }

        public ApontamentoEstornadoEvent(ApontamentoOperacaoEstornadaEventDto apontamentoProducaoEventEventDtoEventDto)
        {
            ApontamentoProducaoEventEventDto = apontamentoProducaoEventEventDtoEventDto;
        }
    }
}