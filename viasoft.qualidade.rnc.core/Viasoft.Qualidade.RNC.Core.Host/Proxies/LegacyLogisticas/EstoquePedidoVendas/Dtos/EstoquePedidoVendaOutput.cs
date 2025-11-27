using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendas.Dtos;

public class EstoquePedidoVendaOutput
{   
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    public Guid IdPedido { get; set; }
    public int LegacyIdEmpresa { get; set; }
    public Guid IdEmpresa { get; set; }
    public decimal Quantidade { get; set; }

}