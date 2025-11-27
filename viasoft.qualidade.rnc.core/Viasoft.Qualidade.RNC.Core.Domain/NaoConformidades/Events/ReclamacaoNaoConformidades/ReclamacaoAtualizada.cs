using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ReclamacaoNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ReclamacaoAtualizada")]
public class ReclamacaoAtualizada : BaseEvent, IEvent
{
    public AlterarReclamacaoNaoConformidadeCommand Command { get; set; }
    
    public ReclamacaoAtualizada()
    {
    }

    public ReclamacaoAtualizada(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}