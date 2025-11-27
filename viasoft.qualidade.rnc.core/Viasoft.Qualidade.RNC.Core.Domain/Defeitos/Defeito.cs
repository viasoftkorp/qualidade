using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.Defeitos;

public class Defeito : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public Guid? IdCausa { get; set; }
    public Guid? IdSolucao { get; set; }
    public bool IsAtivo { get; set; }

    public Defeito()
    {
    }

    public Defeito(DefeitoModel defeito)
    {
        Id = defeito.Id;
        Codigo = defeito.Codigo;
        Descricao = defeito.Descricao;
        Detalhamento = defeito.Detalhamento;
        IdCausa = defeito.IdCausa;
        IdSolucao = defeito.IdSolucao;
    }

    public void Update(DefeitoModel defeito)
    {
        Descricao = defeito.Descricao;
        Detalhamento = defeito.Detalhamento;
        IdCausa = defeito.IdCausa;
        IdSolucao = defeito.IdSolucao;
    }
}