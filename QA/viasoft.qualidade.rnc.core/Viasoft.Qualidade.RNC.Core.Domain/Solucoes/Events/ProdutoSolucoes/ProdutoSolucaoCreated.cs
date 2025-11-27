using System;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Attributes;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;

[Endpoint("Viasoft.Qualidade.RNC.ProdutoSolucaoCreated")]
public class ProdutoSolucaoCreated : IEvent
{
    public ProdutoSolucaoCreated(ProdutoSolucao produtoSolucao, Guid id, DateTime asOfDate, Guid tenantId,
        Guid environmentId)
    {
        IdSolucao = produtoSolucao.IdSolucao;
        IdProduto = produtoSolucao.IdProduto;
        IdProdutoSolucao = produtoSolucao.Id;
        Quantidade = produtoSolucao.Quantidade;
        Id = id;
        AsOfDate = asOfDate;
        TenantId = tenantId;
        EnvironmentId = environmentId;
    }

    public ProdutoSolucaoCreated()
    {
    }

    public Guid Id { get; set; }
    public DateTime AsOfDate { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid IdProduto { get; set; }
    public Guid IdProdutoSolucao { get; set; }
    [StrictRequired] public int Quantidade { get; set; }
}