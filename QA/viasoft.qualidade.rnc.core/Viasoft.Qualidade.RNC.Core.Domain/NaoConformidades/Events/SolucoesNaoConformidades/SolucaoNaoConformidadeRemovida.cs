using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.SolucoesNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.SolucaoNaoConformidadeRemovida")]
public class SolucaoNaoConformidadeRemovida : BaseEvent, IEvent
{
    public Guid IdSolucaoNaoConformidade { get; set; }

    public SolucaoNaoConformidadeRemovida()
    {
    }

    public SolucaoNaoConformidadeRemovida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}