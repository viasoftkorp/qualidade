using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;

public class GerarOperacaoRetrabalhoAclOutput
{
    public List<OperacaoAclOutput> OperacoesAdicionadas { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidationResult { get; set; }
}