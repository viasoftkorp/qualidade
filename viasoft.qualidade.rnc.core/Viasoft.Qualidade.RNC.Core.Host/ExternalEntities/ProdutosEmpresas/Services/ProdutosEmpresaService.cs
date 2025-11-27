using System;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.ProdutosEmpresas;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.ProdutosEmpresas.Services;

public class ProdutoEmpresaService : BaseExternalEntityService<ProdutoEmpresa, ProdutoEmpresaOutput>,
    IProdutoEmpresaService,
    ITransientDependency
{
    public ProdutoEmpresaService(
        IRepository<ProdutoEmpresa> produtosEmpresas,
        IProdutoEmpresaProxyService produtoEmpresaProxyService,
        IUnitOfWork unitOfWork): base(produtosEmpresas, produtoEmpresaProxyService, unitOfWork)
    {
    }

    protected override ProdutoEmpresa ParseProviderOutputToEntity(ProdutoEmpresaOutput outputFromProvider)
    {
        var produtoEmpresa = new ProdutoEmpresa
        {
            Id = outputFromProvider.Id,
            IdProduto = outputFromProvider.ProductId,
            IdEmpresa = outputFromProvider.CompanyId,
            IdCategoria = outputFromProvider.CategoryId
        };

        return produtoEmpresa;
    }
}