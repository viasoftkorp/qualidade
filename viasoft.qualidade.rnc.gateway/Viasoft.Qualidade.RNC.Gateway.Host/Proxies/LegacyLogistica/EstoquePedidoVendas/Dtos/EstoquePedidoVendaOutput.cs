using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendas.Dtos;

public class EstoquePedidoVendaOutput
{   
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    public string NumeroPedido { get; set; }
    public int LegacyIdEmpresa { get; set; }
    public Guid IdEmpresa { get; set; }
    public decimal Quantidade { get; set; }

}