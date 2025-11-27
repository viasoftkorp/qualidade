using System.Threading.Tasks;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.Legacy.Parametros.DTO;
using Viasoft.Core.Legacy.Parametros.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;

public class LegacyParametrosProvider : ILegacyParametrosProvider, ITransientDependency
{
    private readonly ILegacyParametrosService _legacyParametrosService;

    public LegacyParametrosProvider(ILegacyParametrosService legacyParametrosService)
    {
        _legacyParametrosService = legacyParametrosService;
    }
    public async Task<bool> GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque()
    {
        var input = new ReadStringInput
        {
            Chave = "UtilizarReservaDePedidoNaLocalizacaoDeEstoque",
            Secao = "Logistica",
            DefaultValue = "F"
        };

        return await _legacyParametrosService.ReadString(input) == "T";
    }
}