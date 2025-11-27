using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.EngenhariaCore.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.EngenhariaCore.Recursos;

public class RecursoHandler : IHandleMessages<RecursoUpdated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Recurso> _recursos;

    public RecursoHandler(IUnitOfWork unitOfWork, IRepository<Recurso> recursos)
    {
        _unitOfWork = unitOfWork;
        _recursos = recursos;
    }


    public async Task Handle(RecursoUpdated message)
    {
        using (_unitOfWork.Begin())
        {
            await _recursos.BatchUpdateAsync(e => new Recurso
            {
                Descricao = message.Descricao
            }, e => e.Id == message.IdRecurso);
            await _unitOfWork.CompleteAsync();
        }
    }
}