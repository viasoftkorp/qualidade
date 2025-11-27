using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.NaoConformidadeInserida")]
public class NaoConformidadeRemovida : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }

    public NaoConformidadeRemovida()
    {
    }
    public NaoConformidadeRemovida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}