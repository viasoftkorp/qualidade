using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.AcoesPreventivasNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.AcaoPreventivaNaoConformidadeRemovida")]
public class AcaoPreventivaNaoConformidadeRemovida : BaseEvent, IEvent
{
    public Guid IdAcaoPreventiva { get; set; }

    public AcaoPreventivaNaoConformidadeRemovida()
    {
    }

    public AcaoPreventivaNaoConformidadeRemovida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}