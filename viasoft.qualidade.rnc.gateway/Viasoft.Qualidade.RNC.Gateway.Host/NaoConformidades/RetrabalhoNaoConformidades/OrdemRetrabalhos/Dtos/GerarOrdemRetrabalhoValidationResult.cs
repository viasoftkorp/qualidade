namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

public enum GerarOrdemRetrabalhoValidationResult
{
    Ok = 0,
    OperacaoFinalNaoEncontrada = 1,
    OperacaoEngenhariaDuplicada = 2,
    OdfRetrabalhoJaGerada = 3,
    LoteObrigatorio = 4,
    LoteInexistente = 5,
    OdfObrigatorio = 6,
    OdfOrigemIsRetrabalho = 7
}