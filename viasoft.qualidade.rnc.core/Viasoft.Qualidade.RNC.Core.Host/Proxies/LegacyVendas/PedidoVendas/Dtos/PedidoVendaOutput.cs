using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyVendas.PedidoVendas.Dtos;

public class PedidoVendaOutput
{
    public Guid Id { get; set; }
    public Guid IdEmpresa { get; set; }
    public string NumeroPedido { get; set; }
}