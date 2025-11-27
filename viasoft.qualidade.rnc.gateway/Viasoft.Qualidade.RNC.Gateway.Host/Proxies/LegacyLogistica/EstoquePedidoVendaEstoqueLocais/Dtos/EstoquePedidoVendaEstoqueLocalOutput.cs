using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Dtos;

public class EstoquePedidoVendaEstoqueLocalOutput
{   
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    public int LegacyIdEstoquePedidoVenda { get; set; }
    public int LegacyIdEstoqueLocal { get; set; }
}