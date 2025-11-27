using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;

public class ItemNotaFiscalEntradaOutput
{
    public Guid Id { get; set; }
    public int? NumeroNotaFiscal { get; set; }
    public string Lote { get; set; }
    public string CodigoFornecedor { get; set; }
    public Guid IdFornecedor { get; set; }
    public int LegacyNotaId { get; set; }
    public string CodigoProduto { get; set; }
    public Guid IdProduto { get; set; }
}