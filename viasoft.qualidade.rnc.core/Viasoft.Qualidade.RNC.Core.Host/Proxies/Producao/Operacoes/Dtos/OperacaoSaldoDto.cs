namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Dtos;

public class OperacaoSaldoDto
{
    public decimal SaldoUnidadePadrao { get; set; }
    public decimal QuantidadeOperacao999UnidadePadrao { get; set; }
    public decimal QuantidadeProduzidaOperacao999UnidadePadrao { get; set; }
    public decimal SaldoOperacaoToleranciaMaximoUnidadePadrao { get; set; }
    public decimal SaldoOperacaoToleranciaMinimoUnidadePadrao { get; set; }
    public decimal QuantidadeMaximaEncerrarOdfOperacao999UnidadePadrao { get; set; }
    public decimal QuantidadeMinimaEncerrarOdfOperacao999UnidadePadrao { get; set; }

    public decimal Saldo { get; set; }
    public decimal SaldoOperacaoToleranciaMaximo { get; set; }
    public decimal SaldoOperacaoToleranciaMinimo { get; set; }
    public decimal QuantidadeOperacao999 { get; set; }
    public decimal QuantidadeMaximaEncerrarOdfOperacao999 { get; set; }
    public decimal QuantidadeMinimaEncerrarOdfOperacao999 { get; set; }
    public decimal QuantidadeProduzidaOperacao999 { get; set; }

    public bool PrimeiraOperacaoOdf { get; set; }
    public bool DivideNaConversao { get; set; }
    public decimal Tolerancia { get; set; }
    public string Unidade { get; set; }
    public decimal Fator { get; set; }
    public decimal QuantidadeProduzidaOpSecundaria { get; set; }
}