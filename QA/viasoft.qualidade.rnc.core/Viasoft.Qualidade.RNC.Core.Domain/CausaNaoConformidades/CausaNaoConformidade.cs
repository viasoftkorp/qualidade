using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;

public class CausaNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdCausa { get; set; }
    public string Detalhamento { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; } 
    public Guid CompanyId { get; set; }

    public CausaNaoConformidade(CausaNaoConformidade causa)
    {
        Id = causa.Id;
        IdNaoConformidade = causa.IdNaoConformidade;
        IdDefeitoNaoConformidade = causa.IdDefeitoNaoConformidade;
        IdCausa = causa.IdCausa;
        Detalhamento = causa.Detalhamento;
    }

    public CausaNaoConformidade()
    {
    }
}