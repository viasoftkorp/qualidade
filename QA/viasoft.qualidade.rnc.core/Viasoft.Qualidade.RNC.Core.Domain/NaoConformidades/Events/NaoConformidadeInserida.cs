using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.NaoConformidadeInserida")]
public class NaoConformidadeInserida : BaseEvent, IEvent
{
    public NaoConformidadeModel NaoConformidade { get; set; }

    public NaoConformidadeInserida()
    {
    }
    public NaoConformidadeInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}