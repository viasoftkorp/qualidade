using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

public class DefeitoNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeito { get; set; }
    public decimal Quantidade { get; set; }
    public string Detalhamento { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
}