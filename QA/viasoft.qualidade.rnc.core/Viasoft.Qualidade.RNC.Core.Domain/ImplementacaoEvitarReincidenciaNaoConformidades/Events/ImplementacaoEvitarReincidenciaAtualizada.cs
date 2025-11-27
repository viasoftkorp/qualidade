using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.ReclamacaoAtualizada")]
public class ImplementacaoEvitarReincidenciaAtualizada : BaseEvent, IEvent
{
    public AlterarImplementacaoEvitarReincidenciaNaoConformidadeCommand Command { get; set; }
    
    public ImplementacaoEvitarReincidenciaAtualizada()
    {
    }

    public ImplementacaoEvitarReincidenciaAtualizada(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}