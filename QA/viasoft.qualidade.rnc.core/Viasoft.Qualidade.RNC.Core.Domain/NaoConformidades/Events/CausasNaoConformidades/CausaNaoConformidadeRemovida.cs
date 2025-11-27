using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.CausasNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.CausaNaoConformidadeRemovida")]
public class CausaNaoConformidadeRemovida : BaseEvent, IEvent
{
    public Guid IdCausa { get; set; }

    public CausaNaoConformidadeRemovida()
    {
    }

    public CausaNaoConformidadeRemovida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}