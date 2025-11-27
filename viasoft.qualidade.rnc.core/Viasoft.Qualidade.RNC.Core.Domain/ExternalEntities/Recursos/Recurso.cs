using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;

public class Recurso : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public string Descricao { get; set; }
    public string Codigo { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }

    public Recurso()
    {
    }
}