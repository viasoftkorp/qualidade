using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;

public interface IOperacaoService
{
    public Task<bool> ValidarOdfPossuiApontamento(int numeroOdf);
    public Task<OperacaoDto> GetByNumeroOdfENumeroOperacao(int numeroOdf, string numeroOperacao);
    public Task<ApontamentoOperacaoOutput> GetApontamentoOperacaoByLegacyIdOperacao(int legacyIdOperacao);
}    