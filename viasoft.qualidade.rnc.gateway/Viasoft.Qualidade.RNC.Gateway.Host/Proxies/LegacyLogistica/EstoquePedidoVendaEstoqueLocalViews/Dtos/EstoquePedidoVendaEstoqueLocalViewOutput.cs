using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Dtos;

public class EstoquePedidoVendaEstoqueLocalViewOutput
{   
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    public Guid IdEstoquePedidoVenda { get; set; }
    public Guid IdEstoqueLocal { get; set; }
    public bool IsLocalBloquearMovimentacao { get; set; }
    public string NumeroLote { get; set; }
    public int CodigoLocal { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public Guid IdProduto { get; set; }
    public Guid IdPedido { get; set; }
    public decimal Quantidade { get; set; }
    public int NumeroOdf { get; set; }
}