using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;

public class NotaFiscalEntradaOutput
{
    public Guid Id { get; set; }
    public int? NumeroNotaFiscal { get; set; }
    public string CodigoFornecedor { get; set; }
    public Guid IdFornecedor{ get; set; }
    public string Lote { get; set; }
}