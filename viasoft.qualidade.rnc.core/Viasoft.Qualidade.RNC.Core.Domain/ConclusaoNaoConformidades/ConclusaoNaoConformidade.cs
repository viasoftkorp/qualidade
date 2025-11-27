using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;

public class ConclusaoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    public bool NovaReuniao { get; set; }
    public DateTime? DataReuniao { get; set; }
    public DateTime DataVerificacao { get; set; }
    public Guid IdAuditor { get; set; }
    public string Evidencia { get; set; }
    public bool Eficaz { get; set; }
    public int CicloDeTempo { get; set; }
    public Guid IdNovoRelatorio { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }

}