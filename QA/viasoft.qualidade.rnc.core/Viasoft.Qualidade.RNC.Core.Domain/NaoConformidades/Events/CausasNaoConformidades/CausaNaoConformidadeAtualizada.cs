using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.CausasNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.CausaNaoConformidadeAtualizada")]
public class CausaNaoConformidadeAtualizada : BaseEvent, IEvent
{
    public AlterarCausaCommand Command { get; set; }
    
    public CausaNaoConformidadeAtualizada()
    {
    }

    public CausaNaoConformidadeAtualizada(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}