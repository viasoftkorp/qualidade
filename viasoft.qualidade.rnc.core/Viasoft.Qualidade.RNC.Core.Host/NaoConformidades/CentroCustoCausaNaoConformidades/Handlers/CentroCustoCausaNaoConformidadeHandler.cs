using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.CentroCustos.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Handlers;

public class CentroCustoCausaNaoConformidadeHandler: IHandleMessages<CentroCustoCausaNaoConformidadeInserido>
{
    private readonly ICentroCustoService _centroCustoService;

    public CentroCustoCausaNaoConformidadeHandler(ICentroCustoService centroCustoService)
    {
        _centroCustoService = centroCustoService;
    }
    public async Task Handle(CentroCustoCausaNaoConformidadeInserido message)
    {
        await _centroCustoService.InserirSeNaoCadastrado(message.Command.CentroCustoCausaNaoConformidade.IdCentroCusto);
    }
}
