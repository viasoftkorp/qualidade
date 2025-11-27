namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

public enum GerarOrdemRetrabalhoValidationResult
{
    Ok = 0,
    OperacaoFinalNaoEncontrada = 1,
    OperacaoEngenhariaDuplicada = 2,
    OdfRetrabalhoJaGerada = 3,
    LoteObrigatorio = 4,
    OdfObrigatorio = 5,
    QuantidadeInvalida = 6,
    OdfNaoApontada = 7,
    OdfNaoFinalizada = 8,
    RncFechada = 9
}