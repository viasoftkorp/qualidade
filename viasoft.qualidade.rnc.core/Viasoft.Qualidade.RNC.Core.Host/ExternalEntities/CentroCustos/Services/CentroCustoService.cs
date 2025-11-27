using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.CentroCustos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.CentroCustos.Services;

public class CentroCustoService : BaseExternalEntityService<CentroCusto, CentroCustoOutput>, ICentroCustoService, ITransientDependency
{

    public CentroCustoService(IRepository<CentroCusto> centroCustos, ICentroCustoProvider centroCustoProvider,
        IUnitOfWork unitOfWork) : base(centroCustos, centroCustoProvider, unitOfWork)
    {
    }

    protected override CentroCusto ParseProviderOutputToEntity(CentroCustoOutput outputFromProvider)
    {
        var centroCusto = new CentroCusto()
        {
            Id = outputFromProvider.Id,
            Descricao = outputFromProvider.Descricao,
            Codigo = outputFromProvider.Codigo,
            IsSintetico = outputFromProvider.IsSintetico
        };
        return centroCusto;
    }
}