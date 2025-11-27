using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.Operacoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.Operacoes.Providers;

public interface IOperacaoProvider
{
    Task<OperacaoSaldoOutput> GetSaldo(int legacyIdOperacao);
}