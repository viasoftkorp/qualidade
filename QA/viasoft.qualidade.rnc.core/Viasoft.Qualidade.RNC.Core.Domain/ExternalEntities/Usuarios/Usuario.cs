using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;

public class Usuario : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }

    public Usuario()
    {
    }
}