using Newtonsoft.Json;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Events
{
    [Endpoint("Korp.Producao.Apontamento.OrdemEncerrada")]
    public class OrdemProducaoEncerradaEvent : IEvent
    {
        [JsonProperty("ProducaoEncerradaEvent")]
        public OrdemProducaoEncerradaEventDto OrdemProducaoEventEventDto { get; set; }

        public OrdemProducaoEncerradaEvent()
        {
        }

        public OrdemProducaoEncerradaEvent(OrdemProducaoEncerradaEventDto ordemOrdemProducao)
        {
            OrdemProducaoEventEventDto = ordemOrdemProducao;
        }
    }
}