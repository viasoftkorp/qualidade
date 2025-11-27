using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.AclMessaging.ProdutosEmpresas;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.ProdutosEmpresas;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.AclMessaging.ProdutosEmpresas;

public class ProdutoEmpresaHandler : IHandleMessages<ProdutoEmpresaAtualizado>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<ProdutoEmpresa> _produtosEmpresas;

    public ProdutoEmpresaHandler(IUnitOfWork unitOfWork, IRepository<ProdutoEmpresa> produtosEmpresas)
    {
        _unitOfWork = unitOfWork;
        _produtosEmpresas = produtosEmpresas;
    }

    public async Task Handle(ProdutoEmpresaAtualizado message)
    {
        using (_unitOfWork.Begin(options => options.LazyTransactionInitiation = false))
        {
            await _produtosEmpresas.BatchUpdateAsync(produtoEmpresa => new ProdutoEmpresa
            {
                IdCategoria = message.ProdutoEmpresa.IdCategoria
            }, produtoEmpresa =>
                produtoEmpresa.Id == message.ProdutoEmpresa.Id);

            await _unitOfWork.CompleteAsync();
        }
    }
}