using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.AcaoPreventivaNaoConformidadeRemovida")]
public class ImplementacaoEvitarReincidenciaRemovida : BaseEvent, IEvent
{
    public Guid Id { get; set; }

    public ImplementacaoEvitarReincidenciaRemovida()
    {
    }

    public ImplementacaoEvitarReincidenciaRemovida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}