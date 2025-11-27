using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

public class Operacao : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public string NumeroOperacao { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdOperacaoRetrabalhoNaoConformdiade {get; set; }
    public OperacaoRetrabalhoNaoConformidade OperacaoRetrabalhoNaoConformidade { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }

    public void ChangeStatus(StatusProducaoRetrabalho newStatus)
    {
        Status = newStatus;
    }
}