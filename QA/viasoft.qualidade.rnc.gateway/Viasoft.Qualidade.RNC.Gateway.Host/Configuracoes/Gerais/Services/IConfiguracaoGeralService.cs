using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Services;

public interface IConfiguracaoGeralService
{
    public Task<HttpResponseMessage> Get();
    public Task<HttpResponseMessage> Update(ConfiguracaoGeralInput input);
}