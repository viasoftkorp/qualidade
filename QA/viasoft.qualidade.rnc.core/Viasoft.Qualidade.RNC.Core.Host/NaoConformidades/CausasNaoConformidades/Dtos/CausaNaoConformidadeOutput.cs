using System;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

public class CausaNaoConformidadeOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdCausa { get; set; }

    public CausaNaoConformidadeOutput(CausaNaoConformidade causa)
    {
        Id = causa.Id;
        IdNaoConformidade = causa.IdNaoConformidade;
        IdDefeitoNaoConformidade = causa.IdDefeitoNaoConformidade;
        IdCausa = causa.IdCausa;
        Detalhamento = causa.Detalhamento;
    }
}