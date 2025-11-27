using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

public class OperacaoRetrabalhoNaoConformidadeInput
{
    public string NumeroOperacaoARetrabalhar { get; set; }
    public decimal Quantidade { get; set; }
    public List<MaquinaInput> Maquinas { get; set; } = new();
}