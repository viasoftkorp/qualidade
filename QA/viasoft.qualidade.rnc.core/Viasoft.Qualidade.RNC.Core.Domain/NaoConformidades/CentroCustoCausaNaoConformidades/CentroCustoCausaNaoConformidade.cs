using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;

public class CentroCustoCausaNaoConformidade : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid IdCentroCusto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdCausaNaoConformidade { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid CompanyId { get; set; }
}
