using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;

public class GerarOperacaoRetrabalhoAclInput
{
    public IOperacaoRetrabalhoNaoConformidadeModel OperacaoRetrabalhoNaoConformidade { get; set; }
    public int? NumeroOdf { get; set; }
}