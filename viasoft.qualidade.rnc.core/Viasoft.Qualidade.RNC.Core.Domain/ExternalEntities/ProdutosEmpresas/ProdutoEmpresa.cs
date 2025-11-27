using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.ProdutosEmpresas;

public class ProdutoEmpresa : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid IdProduto { get; set; }
    public Guid IdEmpresa { get; set; }
    public Guid IdCategoria { get; set; }

    public ProdutoEmpresa()
    {
    }
}