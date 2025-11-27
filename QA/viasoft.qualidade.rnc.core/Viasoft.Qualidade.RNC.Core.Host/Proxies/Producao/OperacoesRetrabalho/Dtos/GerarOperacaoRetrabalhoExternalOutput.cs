using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;

public class GerarOperacaoRetrabalhoExternalOutput
{
    public List<OperacaoRetrabalhoExternalOutput> OperacoesAdicionadas { get; set; }
    public int? Code { get; set; }
    public string Message { get; set; }
    public bool Success => string.IsNullOrEmpty(Message) && OperacoesAdicionadas != null;
}