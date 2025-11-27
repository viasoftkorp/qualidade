using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.SolucoesNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.SolucaoNaoConformidadeAtualizada")]
public class SolucaoNaoConformidadeAtualizada : BaseEvent, IEvent
{
    public AlterarSolucaoCommand Command { get; set; } 
    public Guid IdSolucaoAnterior { get; set; } 
    
    public SolucaoNaoConformidadeAtualizada()
    {
    }

    public SolucaoNaoConformidadeAtualizada(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}