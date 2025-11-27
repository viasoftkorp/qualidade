using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.AcoesPreventivasNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.AcaoPreventivaNaoConformidadeAtualizada")]
public class AcaoPreventivaNaoConformidadeAtualizada : BaseEvent, IEvent
{
    public AlterarAcaoPreventivaCommand Command { get; set; }
    
    public AcaoPreventivaNaoConformidadeAtualizada()
    {
    }

    public AcaoPreventivaNaoConformidadeAtualizada(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}