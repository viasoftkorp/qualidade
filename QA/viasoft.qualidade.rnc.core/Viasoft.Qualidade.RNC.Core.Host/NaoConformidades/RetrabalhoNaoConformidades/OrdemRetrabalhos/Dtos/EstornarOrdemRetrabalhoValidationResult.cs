namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

public enum EstornarOrdemRetrabalhoValidationResult
{
    Ok = 0,
    OdfRetrabalhoNaoEncontrada = 1,
    OdfRetrabalhoJaApontada = 2,
    RncComOrigemInspecaoSaida = 3,
    RncFechada = 4
}