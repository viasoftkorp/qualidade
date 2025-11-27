using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

public class MovimentarEstoqueListaInput
{
    public decimal Quantidade { get; set; }
    public Guid IdLocalDestino { get; set; }
    public Guid IdLocalOrigem { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public int NumeroOdfOrigem { get; set; }
    public string NumeroPedido { get; set; }
    public string CodigoArmazem { get; set; }
    public Guid IdProduto { get; set; }
    public string NumeroLote { get; set; }
    public int NumeroOdfDestino { get; set; }
}