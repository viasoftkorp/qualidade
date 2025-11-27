using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes;

public class ProdutoSolucao : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid IdProduto { get; set; }
    public int Quantidade { get; set; }
    [IsArrayOfBytes]
    public string OperacaoEngenharia { get; set; }

    public ProdutoSolucao()
    {
        
    }
    public ProdutoSolucao(ProdutoSolucaoModel produtoSolucao)
    {
        Id = produtoSolucao.Id;
        IdProduto = produtoSolucao.IdProduto;
        IdSolucao = produtoSolucao.IdSolucao;
        Quantidade = produtoSolucao.Quantidade;
        OperacaoEngenharia = produtoSolucao.OperacaoEngenharia;
    }
    public ProdutoSolucao(ProdutoSolucao produtoSolucao)
    {
        Id = produtoSolucao.Id;
        IdProduto = produtoSolucao.IdProduto;
        IdSolucao = produtoSolucao.IdSolucao;
        Quantidade = produtoSolucao.Quantidade;
        OperacaoEngenharia = produtoSolucao.OperacaoEngenharia;
    }

    public void Update(ProdutoSolucaoModel produtoSolucao)
    {
        IdProduto = produtoSolucao.IdProduto;
        Quantidade = produtoSolucao.Quantidade;
        OperacaoEngenharia = produtoSolucao.OperacaoEngenharia;
    }
}