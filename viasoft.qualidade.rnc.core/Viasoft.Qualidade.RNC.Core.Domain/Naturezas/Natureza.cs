using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas.Model;


namespace Viasoft.Qualidade.RNC.Core.Domain.Naturezas;

public class Natureza : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    public bool IsAtivo { get; set; }

    public Natureza()
    {
    }

    public Natureza(NaturezaModel natureza)
    {
        Id = natureza.Id;
        Descricao = natureza.Descricao;
        Codigo = natureza.Codigo;
        IsAtivo = natureza.IsAtivo;
    }

    public void Update(NaturezaModel natureza)
    {
        Descricao = natureza.Descricao;
    }
}