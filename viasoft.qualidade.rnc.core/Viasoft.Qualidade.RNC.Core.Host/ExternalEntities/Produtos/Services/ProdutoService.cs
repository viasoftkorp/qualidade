using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.UnidadeMedidaProdutos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;

public class ProdutoService : BaseExternalEntityService<Produto, ProdutoOutput>, IProdutoService, ITransientDependency
{
    private readonly IUnidadeMedidaProdutoService _unidadeMedidaProdutoService;

    public ProdutoService(IProdutosProxyService produtoProxyService, IRepository<Produto> produtos,
        IUnidadeMedidaProdutoService unidadeMedidaProdutoService, IUnitOfWork unitOfWork) : base(produtos, produtoProxyService, unitOfWork)
    {
        _unidadeMedidaProdutoService = unidadeMedidaProdutoService;
    }
    protected override async Task InserirSubEntidades(Produto entity)
    {
        await _unidadeMedidaProdutoService.InserirSeNaoCadastrado(entity.IdUnidadeMedida);
    }    
    
    protected override async Task BatchInserirSubEntidades(List<ProdutoOutput> entities)
    {
        var idsUnidadesMedidasProdutosCadastradas = entities
            .Select(e => e.IdUnidade)
            .Distinct()
            .ToList();
        
        await _unidadeMedidaProdutoService.BatchInserirNaoCadastrados(idsUnidadesMedidasProdutosCadastradas);
        
    }

    protected override Produto ParseProviderOutputToEntity(ProdutoOutput outputFromProvider)
    {
        var produto = new Produto
        {
            Id = outputFromProvider.Id,
            Codigo = outputFromProvider.Codigo,
            Descricao = outputFromProvider.Descricao,
            IdUnidadeMedida = outputFromProvider.IdUnidade,
            IdCategoria = outputFromProvider.IdCategoria
        };
        return produto;
    }
}