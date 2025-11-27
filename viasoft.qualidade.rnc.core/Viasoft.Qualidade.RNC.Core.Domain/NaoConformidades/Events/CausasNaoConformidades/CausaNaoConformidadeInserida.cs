using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.CausasNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.CausaNaoConformidadeInserida")]
public class CausaNaoConformidadeInserida : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }
    public InserirCausaCommand Command { get; set; }

    public CausaNaoConformidadeInserida()
    {
    }

    public CausaNaoConformidadeInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}