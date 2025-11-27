using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;

public class Produto : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public Guid IdUnidadeMedida { get; set; }
    public Guid? IdCategoria { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }

    public Produto()
    {
    }

    public Produto(Produto produto)
    {
        Id = produto.Id;
        Codigo = produto.Codigo;
        Descricao = produto.Descricao;
        IdUnidadeMedida = produto.IdUnidadeMedida;
        IdCategoria = produto.IdCategoria;
        EnvironmentId = produto.EnvironmentId;
        TenantId = produto.TenantId;
    }
}