using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdCategoriaProdutosSeeders.Contracts.Commands;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdCategoriaProdutosSeeders;

public class PreencherIdCategoriaProdutosHandler : IHandleMessages<SeedPreencherIdCategoriaProdutosMessage>
{
    private readonly IRepository<Produto> _produtosRepository;
    private readonly IProdutosProxyService _produtosProxyService;
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly IUnitOfWork _unitOfWork;

    public PreencherIdCategoriaProdutosHandler(IRepository<Produto> produtosRepository,
        IProdutosProxyService produtosProxyService, IRepository<SeederManager> seederManagers,IUnitOfWork unitOfWork)
    {
        _produtosRepository = produtosRepository;
        _produtosProxyService = produtosProxyService;
        _seederManagers = seederManagers;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(SeedPreencherIdCategoriaProdutosMessage message)
    {
        var produtos = await _produtosRepository.ToListAsync();
        var produtosIds = produtos.ConvertAll(e => e.Id);
        var produtosFromLogisticsProducts = await _produtosProxyService.GetAllByIdsPaginando(produtosIds);

        var seederManager = await _seederManagers.FirstAsync();
        seederManager.PreencherIdCategoriaProdutosSeederFinalizado = true;
        var produtosToUpdate = await _produtosRepository.ToListAsync();

        using (_unitOfWork.Begin(op => op.LazyTransactionInitiation = false))
        {
            foreach (var produto in produtosToUpdate)
            {
                produto.IdCategoria = produtosFromLogisticsProducts.First(e => e.Id == produto.Id).IdCategoria;
                await _produtosRepository.UpdateAsync(produto);
            }

            await _seederManagers.UpdateAsync(seederManager);
            await _unitOfWork.CompleteAsync();
        }
    }
}