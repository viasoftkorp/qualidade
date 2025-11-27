using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Handlers;

public class NaoConformidadeHandler : IHandleMessages<CausaNaoConformidadeRemovida>
{
    private readonly IRepository<CentroCustoCausaNaoConformidade> _centrosCustosCausasNaoConformidades;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public NaoConformidadeHandler(
        IRepository<CentroCustoCausaNaoConformidade> centrosCustosCausasNaoConformidades,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _centrosCustosCausasNaoConformidades = centrosCustosCausasNaoConformidades;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CausaNaoConformidadeRemovida message)
    {
        using (_unitOfWork.Begin(options => options.LazyTransactionInitiation = false))
        {
            await _centrosCustosCausasNaoConformidades.BatchUpdateAsync(centroCustoCausaNaoConformidade =>
                new CentroCustoCausaNaoConformidade
                {
                    IsDeleted = true,
                    DeleterId = _currentUser.Id,
                    DeletionTime = message.AsOfDate
                }, centroCustoCausaNaoConformidade => centroCustoCausaNaoConformidade.IdCausaNaoConformidade == message.IdCausa);

            await _unitOfWork.CompleteAsync();
        }
    }
}
