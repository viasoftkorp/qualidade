using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Services;

public class GeracaoCodigoService : IGeracaoCodigoService, ITransientDependency
{
    private readonly IRepository<NaoConformidade> _naoConformidades;

    public GeracaoCodigoService(IRepository<NaoConformidade> naoConformidades)
    {
        _naoConformidades = naoConformidades;
    }

    public async Task<int> GetCodigoNaoConformidade()
    {
        var ultimaInserida = await _naoConformidades
            .AsNoTracking()
            .Where(e => e.Codigo.HasValue)
            .OrderByDescending(entity => entity.CreationTime)
            .FirstOrDefaultAsync();

        var codigo = ultimaInserida != null
            ? ultimaInserida.Codigo.Value + 1
            : 1;

        return codigo;
    }
}