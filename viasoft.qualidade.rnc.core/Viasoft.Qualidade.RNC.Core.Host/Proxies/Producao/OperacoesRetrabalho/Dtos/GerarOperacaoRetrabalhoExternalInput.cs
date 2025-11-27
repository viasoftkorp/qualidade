using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;

public class GerarOperacaoRetrabalhoExternalInput
{
    public Guid IdOrigem { get; set; }
    public string Operacao { get; set; }
    public int Odf { get; set; }
    public decimal SaldoRetrabalhar { get; set; }
    public List<OperacaoRetrabalhoExternalInput> Operacoes { get; set; }
}