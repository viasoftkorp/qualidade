using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;

[Endpoint("Viasoft.Qualidade.RNC.ServicoSolucaoDeleted")]
public class ServicoSolucaoDeleted : IEvent
{
    public ServicoSolucaoDeleted(Solucoes.ServicoSolucao servicoSolucao, Guid id, DateTime asOfDate, Guid tenantId,
        Guid environmentId)
    {
        IdServicoSolucao = servicoSolucao.Id;
        Id = id;
        AsOfDate = asOfDate;
        TenantId = tenantId;
        EnvironmentId = environmentId;
    }

    public ServicoSolucaoDeleted()
    {
        
    }

    public Guid Id { get; set; }
    public DateTime AsOfDate { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdServicoSolucao { get; set; }
}