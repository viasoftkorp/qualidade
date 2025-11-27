using Newtonsoft.Json;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Events
{
    [Endpoint("Viasoft.Sales.CommercialPropostal.LegacyCancelamentoOdf")]
    public class OrdemProducaoCanceladaEvent : IEvent
    {
        [JsonProperty("CancelamentoOdf")]
        public OrdemProducaoCanceladaEventDto OrdemProducaoEventEventDto { get; set; }
        
        public OrdemProducaoCanceladaEvent()
        {
        }

        public OrdemProducaoCanceladaEvent(OrdemProducaoCanceladaEventDto ordemProducaoEventEventDtoEventoEventDto)
        {
            OrdemProducaoEventEventDto = ordemProducaoEventEventDtoEventoEventDto;
        }
    }
}