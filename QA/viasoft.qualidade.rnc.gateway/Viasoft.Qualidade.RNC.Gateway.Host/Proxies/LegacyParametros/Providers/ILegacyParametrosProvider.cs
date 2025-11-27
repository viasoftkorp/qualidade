using System.Threading.Tasks;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyParametros.Providers;

public interface ILegacyParametrosProvider
{
    public Task<bool> GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();
}