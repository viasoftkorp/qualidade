namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.Operacoes.Dtos;

public class OperacaoSaldoOutput
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

    public OperacaoSaldoOutput()
    {
    }

    public OperacaoSaldoOutput(GetApontamentoOperacaoOutput apontamentoOperacaoOutput)
    {
        var saldoOp = apontamentoOperacaoOutput.Operacao.OperacaoSaldoDTO;

        SaldoUnidadePadrao = saldoOp.SaldoUnidadePadrao;
        QuantidadeOperacao999UnidadePadrao = saldoOp.QuantidadeOperacao999UnidadePadrao;
        QuantidadeProduzidaOperacao999UnidadePadrao = saldoOp.QuantidadeProduzidaOperacao999UnidadePadrao;
        SaldoOperacaoToleranciaMaximoUnidadePadrao = saldoOp.SaldoOperacaoToleranciaMaximoUnidadePadrao;
        SaldoOperacaoToleranciaMinimoUnidadePadrao = saldoOp.SaldoOperacaoToleranciaMinimoUnidadePadrao;
        QuantidadeMaximaEncerrarOdfOperacao999UnidadePadrao =
            saldoOp.QuantidadeMaximaEncerrarOdfOperacao999UnidadePadrao;
        QuantidadeMinimaEncerrarOdfOperacao999UnidadePadrao =
            saldoOp.QuantidadeMinimaEncerrarOdfOperacao999UnidadePadrao;
        Saldo = saldoOp.Saldo;
        SaldoOperacaoToleranciaMaximo = saldoOp.SaldoOperacaoToleranciaMaximo;
        SaldoOperacaoToleranciaMinimo = saldoOp.SaldoOperacaoToleranciaMinimo;
        QuantidadeOperacao999 = saldoOp.QuantidadeOperacao999;
        QuantidadeMaximaEncerrarOdfOperacao999 = saldoOp.QuantidadeMaximaEncerrarOdfOperacao999;
        QuantidadeMinimaEncerrarOdfOperacao999 = saldoOp.QuantidadeMinimaEncerrarOdfOperacao999;
        QuantidadeProduzidaOperacao999 = saldoOp.QuantidadeProduzidaOperacao999;
        PrimeiraOperacaoOdf = saldoOp.PrimeiraOperacaoOdf;
        DivideNaConversao = saldoOp.DivideNaConversao;
        Tolerancia = saldoOp.Tolerancia;
        Unidade = saldoOp.Unidade;
        Fator = saldoOp.Fator;
        QuantidadeProduzidaOpSecundaria = saldoOp.QuantidadeProduzidaOpSecundaria;
    }
}

public class GetApontamentoOperacaoOutput
{
    public OperacaoDto Operacao { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
}

public class OperacaoDto
{
    public GetOperacaoSaldoDto OperacaoSaldoDTO { get; set; }
}

public class GetOperacaoSaldoDto
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