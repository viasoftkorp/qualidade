using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;

public class CausaNaoConformidadeInput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdCausa { get; set; }
    public List<Guid> IdsCentrosCustos { get; set; } = new();
}
