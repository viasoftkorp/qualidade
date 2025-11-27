using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Recursos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Recursos.Services;

public class RecursoService : BaseExternalEntityService<Recurso, RecursoOutput>, IRecursoService, ITransientDependency
{
    public RecursoService(IRepository<Recurso> repository, IRecursosProxyService provider, IUnitOfWork unitOfWork) : base(repository, provider, unitOfWork)
    {
    }

    protected override Recurso ParseProviderOutputToEntity(RecursoOutput outputFromProvider)
    {
        var recurso = new Recurso
        {
            Id = outputFromProvider.Id,
            Descricao = outputFromProvider.Descricao,
            Codigo = outputFromProvider.Codigo,
        };
        return recurso;
    }
}