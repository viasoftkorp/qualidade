using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;

public class AcaoPreventiva : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }

    public Guid EnvironmentId { get; set; }

    public int Codigo { get; set; }

    public string Descricao { get; set; }
    [IsArrayOfBytes]

    public string Detalhamento { get; set; }

    public Guid? IdResponsavel { get; set; }
    public bool IsAtivo { get; set; }

    public AcaoPreventiva()
    {
    }

    public AcaoPreventiva(AcaoPreventivaModel acaoPreventiva)
    {
        Id = acaoPreventiva.Id;
        Codigo = acaoPreventiva.Codigo;
        Descricao = acaoPreventiva.Descricao;
        Detalhamento = acaoPreventiva.Detalhamento;
        IdResponsavel = acaoPreventiva.IdResponsavel;
        IsAtivo = acaoPreventiva.IsAtivo;
    }
    
    public void Update(AcaoPreventivaModel acaoPreventiva)
    {
        Descricao = acaoPreventiva.Descricao;
        Detalhamento = acaoPreventiva.Detalhamento;
        IdResponsavel = acaoPreventiva.IdResponsavel;
    }
    
}