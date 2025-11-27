using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Clientes;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Pessoas.Services;

public class PessoaService : BaseExternalEntityService<Cliente, PersonOutput>, IPessoaService, ITransientDependency
{
    public PessoaService(IRepository<Cliente> repository, IPersonProxyService provider, IUnitOfWork unitOfWork) : base(repository, provider, unitOfWork)
    {
    }

    protected override Cliente ParseProviderOutputToEntity(PersonOutput outputFromProvider)
    {
        var cliente = new Cliente
        {
            Id = outputFromProvider.Id,
            Codigo = outputFromProvider.Code,
            RazaoSocial = outputFromProvider.CompanyName,
        };
        return cliente;
    }
}