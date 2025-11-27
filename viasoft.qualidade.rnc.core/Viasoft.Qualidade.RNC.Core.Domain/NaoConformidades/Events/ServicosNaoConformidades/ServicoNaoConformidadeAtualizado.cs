using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ServicosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ServicoSolucaoNaoConformidadeAtualizado")]
public class ServicoNaoConformidadeAtualizado : BaseEvent, IEvent
{
    public AlterarServicosNaoConformidadeCommand Command { get; set; }
    
    public ServicoNaoConformidadeAtualizado()
    {
    }

    public ServicoNaoConformidadeAtualizado(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}