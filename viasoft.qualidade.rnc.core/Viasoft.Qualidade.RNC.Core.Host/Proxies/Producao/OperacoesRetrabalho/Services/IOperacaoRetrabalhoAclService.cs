using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;

public interface IOperacaoRetrabalhoAclService
{
    public Task<GerarOperacaoRetrabalhoExternalInput> GetGerarOperacaoRetrabalhoExternalInput(GerarOperacaoRetrabalhoAclInput input, 
        List<MaquinaInput> maquinasInput);

    public Task<GerarOperacaoRetrabalhoAclOutput> GetGerarOperacaoRetrabalhoAclOutput(
        GerarOperacaoRetrabalhoExternalOutput gerarOperacaoRetrabalhoExternalOutput);
}