namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

public enum OperacaoRetrabalhoNaoConformidadeValidationResult
{
    Ok = 0,
    OperacaoRetrabalhoJaExiste = 1,
    NenhumMaquinaCadastrada = 2,
    OdfNaoApontada = 3,
    OdfFinalizada = 4,
    GerarRetrabalhoOp999 = 5,
    GerarRetrabalhoOpRetrabalho = 6,
    GerarRetrabalhoOpSecundariaSaldoOdfZerado = 7,
    GerarRetrabalhoOpSecundariaTamanhoOperacoesNaoPadronizados = 8,
    GerarRetrabalhoOpSecundariaLimiteOperacoesAtingido = 9,
    GerarRetrabalhoOpSecundariaSaldoOperacaoIndisponivel = 10,
    GerarRetrabalhoOpSecundariaSaldoOperacaoInsuficiente = 11,
    RncFechada = 12
}