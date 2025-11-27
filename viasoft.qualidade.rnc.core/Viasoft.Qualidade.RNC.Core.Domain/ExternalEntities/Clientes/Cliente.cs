using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;

public class Cliente : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public string Codigo { get; set; }
    public string RazaoSocial { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }

    public Cliente()
    {
    }
    
}