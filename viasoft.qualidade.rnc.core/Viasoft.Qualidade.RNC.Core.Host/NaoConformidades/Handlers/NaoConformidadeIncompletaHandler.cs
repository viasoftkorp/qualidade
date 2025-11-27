using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Handlers;

public class NaoConformidadeIncompletaHandler : IHandleMessages<VerificarNaoConformidadeIncompletaMessage>
{
    private readonly IRepository<NaoConformidade> _naoConformidades;

    public NaoConformidadeIncompletaHandler(IRepository<NaoConformidade> naoConformidades)
    {
        _naoConformidades = naoConformidades;
    }
    public async Task Handle(VerificarNaoConformidadeIncompletaMessage message)
    {
        var naoConformidade = await _naoConformidades.FirstOrDefaultAsync(e => e.Id == message.IdNaoConformidade);
        if (naoConformidade != null && naoConformidade.Incompleta)
        {
            await _naoConformidades.DeleteAsync(naoConformidade, true);
        }
    }
}