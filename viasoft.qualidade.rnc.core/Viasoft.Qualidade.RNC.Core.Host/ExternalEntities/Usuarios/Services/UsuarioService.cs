using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Usuarios;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;

public class UsuarioService : BaseExternalEntityService<Usuario, UsuarioOutput>, IUsuarioService, ITransientDependency
{
    protected override Usuario ParseProviderOutputToEntity(UsuarioOutput outputFromProvider)
    {
        var usuario = new Usuario
        {
            Id = outputFromProvider.Id,
            Nome = outputFromProvider.FirstName,
            Sobrenome = outputFromProvider.SecondName
        };
        return usuario;
    }

    public UsuarioService(IRepository<Usuario> repository, IUsuarioProxyService provider, IUnitOfWork unitOfWork) : base(repository, provider, unitOfWork)
    {
    }
}