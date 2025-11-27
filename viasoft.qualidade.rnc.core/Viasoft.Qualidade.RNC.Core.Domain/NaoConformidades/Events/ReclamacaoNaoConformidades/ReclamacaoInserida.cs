using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ReclamacaoNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ReclamacaoInserida")]
public class ReclamacaoInserida : BaseEvent, IEvent
{
    public InserirReclamacaoNaoConformidadeCommand Command { get; set; }
    
    public ReclamacaoInserida()
    {
    }

    public ReclamacaoInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}