using Newtonsoft.Json;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events;

[Endpoint("Korp.Producao.Apontamento.OperacaoRemovida")]
public class ApontamentoOperacaoRemovidaEvent : IEvent
{
    [JsonProperty("OperacaoRemovidaDto")]
    public ApontamentoOperacaoRemovidaEventDto ApontamentoProducaoEventEventDto { get; set; }
    
    public ApontamentoOperacaoRemovidaEvent()
    {
        
    }

    public ApontamentoOperacaoRemovidaEvent(ApontamentoOperacaoRemovidaEventDto apontamentoProducaoEventEventDto)
    {
        ApontamentoProducaoEventEventDto = apontamentoProducaoEventEventDto;
    }
}