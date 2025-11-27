using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.DefeitosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.DefeitoNaoConformidadeRemovido")]
public class DefeitoNaoConformidadeRemovido : BaseEvent, IEvent
{
    public Guid IdDefeito { get; set; }

    public DefeitoNaoConformidadeRemovido()
    {
    }

    public DefeitoNaoConformidadeRemovido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}