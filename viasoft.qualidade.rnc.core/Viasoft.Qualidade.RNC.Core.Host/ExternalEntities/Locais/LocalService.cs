using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Locais;

public class LocalService : BaseExternalEntityService<Local, LocalOutput>, ILocalService, ITransientDependency
{
    public LocalService(IRepository<Local> repository, ILocalProvider provider, IUnitOfWork unitOfWork) 
        : base(repository, provider, unitOfWork)
    {
    }

    protected override Local ParseProviderOutputToEntity(LocalOutput outputFromProvider)
    {
        var output = new Local
        {
            Id = outputFromProvider.Id,
            Descricao = outputFromProvider.Descricao,
            Codigo = outputFromProvider.Codigo,
            IsBloquearMovimentacao = outputFromProvider.IsBloquearMovimentacao
        };
        return output;
    }
}