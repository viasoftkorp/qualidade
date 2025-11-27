using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.CentroCustoCausaNaoConformidadeRemovido")]
public class CentroCustoCausaNaoConformidadeRemovido :BaseEvent, IEvent
{
    public Guid Id { get; set; }
    
    public CentroCustoCausaNaoConformidadeRemovido()
    {
    }

    public CentroCustoCausaNaoConformidadeRemovido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}
