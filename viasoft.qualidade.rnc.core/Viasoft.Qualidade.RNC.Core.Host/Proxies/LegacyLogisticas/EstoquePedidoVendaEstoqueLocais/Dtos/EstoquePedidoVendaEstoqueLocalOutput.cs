using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendaEstoqueLocais.Dtos;

public class EstoquePedidoVendaEstoqueLocalOutput
{   
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    
    public Guid IdEstoquePedidoVenda { get; set; }
    public Guid IdEstoqueLocal { get; set; }
    public decimal Quantidade { get; set; }
}