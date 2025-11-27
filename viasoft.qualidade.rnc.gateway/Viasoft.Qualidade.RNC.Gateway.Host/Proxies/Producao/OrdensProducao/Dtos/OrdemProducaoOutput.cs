using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;

public class OrdemProducaoOutput
{
    public int NumeroOdf { get; set; }

    public string Revisao { get; set; }
    public Guid IdProduto { get; set; }
    public decimal Quantidade { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime DataEntrega { get; set; }
    public string NumeroPedido { get; set; }
    public string Observacao { get; set; }
    public bool IsRetrabalho { get; set; }
    public int? NumeroOdfDestino { get; set; }
    public int NumeroOdfFaturamento { get; set; }
    public Guid? IdProdutoFaturamento { get; set; }
    public string NumeroLote { get; set; }
    public bool OdfFinalizada { get; set; }
    public bool PossuiPartida { get; set; }
    public Guid? IdCliente { get; set; }

}