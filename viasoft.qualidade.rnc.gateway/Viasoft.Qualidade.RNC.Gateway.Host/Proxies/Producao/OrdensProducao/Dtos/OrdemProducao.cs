namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;

public class OrdemProducao
{
    public int Odf { get; set; }
    public string Revisao { get; set; }
    public ProdutoDto ProdutoDTO { get; set; }
    public decimal QuantidadeOrdem { get; set; }
    public decimal QuantidadeProduzida { get; set; }
    public decimal QuantidadeAProduzir { get; set; }
    public bool OrdemServico { get; set; }
    public int IdRoteiro { get; set; }
    public int CodigoRoteiro { get; set; }
    public string DescricaoRoteiro { get; set; }
    public int IdProcesso { get; set; }
    public string OrdemEncerrada { get; set; }
    public string DataInicio { get; set; }
    public string DataEntrega { get; set; }
    public string DataNegociada { get; set; }
    public bool UtilizaCracha { get; set; }
    public string PedidoVenda { get; set; }
    public string Servico { get; set; }
    public string DestinacaoODF { get; set; }
    public string NumeroPedido { get; set; }
    public string Programador { get; set; }
    public string Planejador { get; set; }
    public string Observacao { get; set; }
    public bool Retrabalho { get; set; }
    public int OdfDestino { get; set; }
    public int OdfFaturamento { get; set; }
    public string CodigoProdutoFaturamento { get; set; }
    public string Lote { get; set; }
    public bool OrdemFechada { get; set; }
    public string CodigoCliente { get; set; }

}

public class ProdutoDto
{
    public string Codigo { get; set; }
}