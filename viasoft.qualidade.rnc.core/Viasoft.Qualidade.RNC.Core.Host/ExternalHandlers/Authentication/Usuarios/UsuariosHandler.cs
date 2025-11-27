using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.Authentication.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.Authentication.Usuarios;

public class UsuariosHandler : IHandleMessages<ApplicationUserUpdated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Usuario> _usuarios;

    public UsuariosHandler(IUnitOfWork unitOfWork, IRepository<Usuario> usuarios)
    {
        _unitOfWork = unitOfWork;
        _usuarios = usuarios;
    }
    public async Task Handle(ApplicationUserUpdated message)
    {
        using (_unitOfWork.Begin())
        {
            await _usuarios.BatchUpdateAsync(e => new Usuario
            {
                Nome = message.FirstName,
                Sobrenome = message.SecondName
            }, e => e.Id == message.Id);
           
            await _unitOfWork.CompleteAsync();
        }
    }
}