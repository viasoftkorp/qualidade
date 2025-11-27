using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Events;

[Endpoint("Viasoft.Qualidade.RNC.DefeitoDeleted")]
public class DefeitoDeleted : IEvent
{
    public DefeitoDeleted(Defeito defeito, Guid id, DateTime asOfDate, Guid tenantId,
        Guid environmentId)
    {
        IdDefeito = defeito.Id;
        Id = id;
        AsOfDate = asOfDate;
        TenantId = tenantId;
        EnvironmentId = environmentId;
    }

    public DefeitoDeleted()
    {
    }

    public Guid Id { get; set; }
    public DateTime AsOfDate { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdDefeito { get; set; }
    

   
}