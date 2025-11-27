using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.AcoesPreventivasNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.AcaoPreventivaNaoConformidadeInserida")]
public class AcaoPreventivaNaoConformidadeInserida : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }
    public InserirAcaoPreventivaCommand Command { get; set; }

    public AcaoPreventivaNaoConformidadeInserida()
    {
    }

    public AcaoPreventivaNaoConformidadeInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}