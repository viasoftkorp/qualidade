using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;

public class ProdutoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdProduto { get; set; }
    public Guid IdNaoConformidade { get; set; }

    public string Detalhamento { get; set; }
    public string OperacaoEngenharia { get; set; }
    public decimal Quantidade { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }

    public ProdutoNaoConformidade()
    {
    }

    public ProdutoNaoConformidade(ProdutoNaoConformidade produto)
    {
        Id = produto.Id;
        IdProduto = produto.IdProduto;
        IdNaoConformidade = produto.IdNaoConformidade;
        Detalhamento = produto.Detalhamento;
        Quantidade = produto.Quantidade;
        OperacaoEngenharia = produto.OperacaoEngenharia;
    }
}