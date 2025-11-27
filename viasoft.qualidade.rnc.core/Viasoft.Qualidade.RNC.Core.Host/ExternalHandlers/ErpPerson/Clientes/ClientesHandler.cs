using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ErpPerson.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.ErpPerson.Clientes;

public class ClientesHandler : IHandleMessages<PersonUpdated>
{
    private readonly IRepository<Cliente> _clientes;
    private readonly IUnitOfWork _unitOfWork;

    public ClientesHandler(IRepository<Cliente> clientes, IUnitOfWork unitOfWork)
    {
        _clientes = clientes;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(PersonUpdated message)
    {
        using (_unitOfWork.Begin())
        {
            await _clientes.BatchUpdateAsync(e => new Cliente
            {
                Codigo = message.Person.Code,
                RazaoSocial = message.Person.CompanyName
            }, e => e.Id == message.Person.Id);
           
            await _unitOfWork.CompleteAsync();
        }
    }
}