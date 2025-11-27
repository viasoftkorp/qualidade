using System;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;

public class Local: Entity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public bool? IsBloquearMovimentacao { get; set; }
}