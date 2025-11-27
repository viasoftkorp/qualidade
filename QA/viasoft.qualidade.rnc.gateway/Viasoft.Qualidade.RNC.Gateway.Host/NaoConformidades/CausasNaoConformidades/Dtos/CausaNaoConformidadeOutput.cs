using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;

public class CausaNaoConformidadeOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdCausa { get; set; }

    public CausaNaoConformidadeOutput()
    {
        Id = Id;
        IdNaoConformidade = IdNaoConformidade;
        IdDefeitoNaoConformidade = IdDefeitoNaoConformidade;
        Detalhamento = Detalhamento;
        IdCausa = IdCausa;
    }
}