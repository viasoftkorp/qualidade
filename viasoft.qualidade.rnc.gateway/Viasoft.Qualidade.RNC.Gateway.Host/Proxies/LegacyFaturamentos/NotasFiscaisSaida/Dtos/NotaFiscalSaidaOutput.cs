using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;

public class NotaFiscalSaidaOutput
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public int? NumeroNotaFiscal { get; set; }

}