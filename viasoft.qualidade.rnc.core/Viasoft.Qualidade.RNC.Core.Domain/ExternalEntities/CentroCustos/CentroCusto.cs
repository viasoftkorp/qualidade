using System;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.CentroCustos;

public class CentroCusto: Entity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public string Descricao{ get; set; }
    public string Codigo { get; set; }
    public bool IsSintetico { get; set; }
}