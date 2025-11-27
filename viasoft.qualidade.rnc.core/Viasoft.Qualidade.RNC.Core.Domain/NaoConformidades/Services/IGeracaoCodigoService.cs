using System.Threading.Tasks;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Services;

public interface IGeracaoCodigoService
{
    public Task<int> GetCodigoNaoConformidade();
}