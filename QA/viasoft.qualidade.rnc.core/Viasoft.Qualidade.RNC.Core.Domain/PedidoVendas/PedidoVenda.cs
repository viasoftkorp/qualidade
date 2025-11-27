using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.PedidoVendas;

public class PedidoVenda : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public string NumeroPedido { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
}