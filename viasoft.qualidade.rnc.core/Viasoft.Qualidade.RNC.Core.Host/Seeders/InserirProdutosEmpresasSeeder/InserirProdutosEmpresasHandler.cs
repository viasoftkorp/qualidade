using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.ProdutosEmpresas;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.InserirProdutosEmpresasSeeder;

public class InserirProdutosEmpresasSeederHandler :
    IHandleMessages<InserirProdutosEmpresasSeederMessage>
{
    private readonly IRepository<SeederManagerPorEmpresa> _seederManagerPorEmpresas;
    private readonly IRepository<Produto> _produtos;
    private readonly IRepository<ProdutoEmpresa> _produtosEmpresas;
    private readonly IProdutoEmpresaProxyService _produtoEmpresaProxyService;
    private readonly IAmbientData _ambientData;
    private readonly IUnitOfWork _unitOfWork;
    private const int MaxQueryCount = 200;
    private const int MaxQueryParamsCount = 50;

    public InserirProdutosEmpresasSeederHandler(
        IRepository<SeederManagerPorEmpresa> seederManagerPorEmpresas,
        IRepository<Produto> produtos,
        IRepository<ProdutoEmpresa> produtosEmpresas,
        IProdutoEmpresaProxyService produtoEmpresaProxyService,
        IAmbientData ambientData,
        IUnitOfWork unitOfWork)
    {
        _seederManagerPorEmpresas = seederManagerPorEmpresas;
        _produtos = produtos;
        _produtosEmpresas = produtosEmpresas;
        _produtoEmpresaProxyService = produtoEmpresaProxyService;
        _ambientData = ambientData;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(InserirProdutosEmpresasSeederMessage message)
    {
        var skipCount = 0;

        while (true)
        {
            var produtos = await _produtos
                .AsNoTracking()
                .OrderBy(produto => produto.Id)
                .PageBy(skipCount, MaxQueryCount)
                .ToListAsync();

            if (!produtos.Any())
            {
                break;
            }

            var produtoEmpresaAtualPorProduto = await GetProdutosEmpresas(produtos);

            var produtosEmpresas = produtos
                .Select(produto =>
                {
                    var produtoEmpresa = produtoEmpresaAtualPorProduto[produto.Id];

                    return new ProdutoEmpresa
                    {
                        Id = produtoEmpresa.Id,
                        IdProduto = produtoEmpresa.ProductId,
                        IdEmpresa = produtoEmpresa.CompanyId,
                        IdCategoria = produtoEmpresa.CategoryId
                    };
                })
                .ToList();

            using (_unitOfWork.Begin())
            {
                await _produtosEmpresas.InsertRangeAsync(produtosEmpresas);
                await _unitOfWork.CompleteAsync();
            }

            skipCount += MaxQueryCount;
        }

        var seederManager = await _seederManagerPorEmpresas.FirstAsync();
        seederManager.InserirProdutosEmpresasSeederFinalizado = true;
        await _seederManagerPorEmpresas.UpdateAsync(seederManager, true);
    }

    private async Task<Dictionary<Guid, ProdutoEmpresaOutput>> GetProdutosEmpresas(List<Produto> produtos)
    {
        var skipCount = 0;
        var categoriaPorProduto = new Dictionary<Guid, ProdutoEmpresaOutput>();
        var idsProdutos = produtos.ConvertAll(produto => produto.Id);

        while (true)
        {
            var idsProdutosParaBuscar = idsProdutos
                .Skip(skipCount)
                .Take(MaxQueryParamsCount)
                .ToList();

            if (!idsProdutosParaBuscar.Any())
            {
                break;
            }

            var input = new GetProdutoEmpresaListInput
            {
                ProductsIds = idsProdutosParaBuscar,
                CompanyId = _ambientData.GetCompanyId(),
                MaxResultCount = MaxQueryParamsCount
            };

            var pagedResult = await _produtoEmpresaProxyService.GetAll(input);

            foreach (var produtoEmpresa in pagedResult.Items)
            {
                categoriaPorProduto.Add(produtoEmpresa.ProductId, produtoEmpresa);
            }

            skipCount += MaxQueryParamsCount;
        }

        return categoriaPorProduto;
    }
}