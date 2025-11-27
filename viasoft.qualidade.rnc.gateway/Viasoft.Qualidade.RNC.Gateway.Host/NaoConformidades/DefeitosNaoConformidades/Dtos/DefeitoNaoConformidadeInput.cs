using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

public class DefeitoNaoConformidadeInput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdDefeito { get; set; }
    public decimal Quantidade { get; set; }
}