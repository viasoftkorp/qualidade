using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Dtos;

public class ItemNotaFiscalEntradaRateioLoteOutput
{
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    public int LegacyIdItemNotaFiscal { get; set; }
    public string Lote { get; set; }
}