using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;

public class CausaNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdCausa { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
}