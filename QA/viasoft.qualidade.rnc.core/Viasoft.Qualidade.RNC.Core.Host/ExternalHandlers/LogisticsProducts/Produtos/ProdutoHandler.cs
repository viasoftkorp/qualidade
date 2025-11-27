using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsProducts.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.LogisticsProducts.Produtos;

public class ProdutoHandler : IHandleMessages<ProductUpdated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Produto> _produtos;

    public ProdutoHandler(IUnitOfWork unitOfWork, IRepository<Produto> produtos)
    {
        _unitOfWork = unitOfWork;
        _produtos = produtos;
    }
    
    
    public async Task Handle(ProductUpdated message)
    {
        using (_unitOfWork.Begin())
        {
            await _produtos.BatchUpdateAsync(e => new Produto
            {
                Codigo = message.Product.Codigo,
                Descricao = message.Product.Descricao
            }, e => e.Id == message.Product.Id);
           
            await _unitOfWork.CompleteAsync();
        }
    }
    
}