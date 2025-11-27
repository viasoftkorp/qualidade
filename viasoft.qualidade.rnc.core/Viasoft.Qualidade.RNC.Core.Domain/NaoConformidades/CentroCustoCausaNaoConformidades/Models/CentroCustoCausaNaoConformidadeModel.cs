using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Models;

public class CentroCustoCausaNaoConformidadeModel : ICentroCustoCausaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid CompanyId { get; set; }
    public Guid IdCentroCusto { get; set; }
    public Guid IdCausaNaoConformidade { get; set; }

    public CentroCustoCausaNaoConformidadeModel()
    {

    }

    public CentroCustoCausaNaoConformidadeModel(ICentroCustoCausaNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        IdCentroCusto = model.IdCentroCusto;
        IdCausaNaoConformidade = model.IdCausaNaoConformidade;
        CompanyId = model.CompanyId;
    }
}
