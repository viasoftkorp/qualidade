using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;

public class CausaNaoConformidadeModel : ICausaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }

    public string Detalhamento { get; set; }
    public Guid IdCausa { get; set; }
    public Guid CompanyId { get; set; }
    public List<Guid> IdsCentrosCustos { get; set; } = new();

    public CausaNaoConformidadeModel()
    {
    }

    public CausaNaoConformidadeModel(ICausaNaoConformidadeModel model)
    {
        IdNaoConformidade = model.IdNaoConformidade;
        Detalhamento = model.Detalhamento;
        IdDefeitoNaoConformidade = model.IdDefeitoNaoConformidade;
        IdCausa = model.IdCausa;
        CompanyId = model.CompanyId;
        IdsCentrosCustos = model.IdsCentrosCustos;
    }
}
