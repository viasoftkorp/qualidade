using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Causas.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.Causas;

public class Causa : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public string Descricao { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public int Codigo { get; set; } 
    public bool IsAtivo { get; set; }

    public Causa()
    {
    }

    public Causa(CausaModel causa)
    {
        Id = causa.Id;
        Descricao = causa.Descricao;
        Codigo = causa.Codigo;
        Detalhamento = causa.Detalhamento;
        IsAtivo = causa.IsAtivo;
    }

    public void Update(CausaModel causa)
    {
        Descricao = causa.Descricao;
        Detalhamento = causa.Detalhamento;
    }
}