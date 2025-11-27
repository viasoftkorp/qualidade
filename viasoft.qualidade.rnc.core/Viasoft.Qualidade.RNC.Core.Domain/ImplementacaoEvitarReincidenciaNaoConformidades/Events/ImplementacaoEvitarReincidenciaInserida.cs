using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.ImplementacaoEvitarReincidenciaInserida")]
public class ImplementacaoEvitarReincidenciaInserida : BaseEvent, IEvent
{
    public InserirImplementacaoEvitarReincidenciaNaoConformidadeCommand Command { get; set; }
    
    public ImplementacaoEvitarReincidenciaInserida()
    {
    }

    public ImplementacaoEvitarReincidenciaInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}