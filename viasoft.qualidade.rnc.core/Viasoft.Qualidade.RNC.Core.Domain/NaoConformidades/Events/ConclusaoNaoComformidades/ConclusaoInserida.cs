using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ConclusaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ConclusaoNaoComformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ConclusaoInserida")]
public class ConclusaoInserida : BaseEvent, IEvent
{
    public ConcluirNaoConformidadeCommand Command { get; set; }
    
    public ConclusaoInserida()
    {
    }

    public ConclusaoInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}