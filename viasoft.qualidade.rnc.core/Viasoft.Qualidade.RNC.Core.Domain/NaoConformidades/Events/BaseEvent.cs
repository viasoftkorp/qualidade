using System;
using Viasoft.Core.DateTimeProvider;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events
{
    public abstract class BaseEvent
    {
        public BaseEvent()
        {
            
        }
        public BaseEvent(IDateTimeProvider dateTimeProvider, Guid tenantId, Guid environmentId, Guid? eventId)
        {
            AsOfDate = dateTimeProvider.UtcNow();
            TenantId = tenantId;
            EnvironmentId = environmentId;
            EventId = eventId ?? Guid.NewGuid();
        }
        public Guid EventId { get; init; }
        public Guid TenantId { get; init; }
        public Guid EnvironmentId { get; init; }
        public DateTime AsOfDate { get; init; }
    }
}