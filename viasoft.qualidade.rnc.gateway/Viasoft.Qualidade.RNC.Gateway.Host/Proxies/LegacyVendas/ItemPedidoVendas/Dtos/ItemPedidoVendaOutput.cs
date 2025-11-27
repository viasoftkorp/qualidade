using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyVendas.ItemPedidoVendas.Dtos;

public class ItemPedidoVendaOutput
{
    public int Odf { get; set; }
    public Guid IdPedido { get; set; }
    public string NumeroPedido { get; set; }
}
