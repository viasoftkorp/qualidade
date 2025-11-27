using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;

[Endpoint("Viasoft.Qualidade.RNC.ProdutoSolucaoDeleted")]
public class ProdutoSolucaoDeleted : IEvent
{
    public ProdutoSolucaoDeleted(ProdutoSolucao produtoSolucao, Guid id, DateTime asOfDate, Guid tenantId,
        Guid environmentId)
    {
        IdProdutoSolucao = produtoSolucao.Id;
        Id = id;
        AsOfDate = asOfDate;
        TenantId = tenantId;
        EnvironmentId = environmentId;
    }

    public ProdutoSolucaoDeleted()
    {
    }

    public Guid Id { get; set; }
    public DateTime AsOfDate { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdProdutoSolucao { get; set; }
}