using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.UnidadeMedidaProdutos.Services;

public class UnidadeMedidaProdutoService : BaseExternalEntityService<UnidadeMedidaProduto, UnidadeMedidaProdutoOutput>, IUnidadeMedidaProdutoService, ITransientDependency
{
    public UnidadeMedidaProdutoService(IRepository<UnidadeMedidaProduto> repository, IUnidadeMedidaProdutoProxyService provider, IUnitOfWork unitOfWork) : base(repository, provider, unitOfWork)
    {
    }

    protected override UnidadeMedidaProduto ParseProviderOutputToEntity(UnidadeMedidaProdutoOutput outputFromProvider)
    {
        var unidadeMedidaProduto = new UnidadeMedidaProduto
        {
            Id = outputFromProvider.Id,
            Descricao = outputFromProvider.UnidadeMedida,
        };
        return unidadeMedidaProduto;
    }
}