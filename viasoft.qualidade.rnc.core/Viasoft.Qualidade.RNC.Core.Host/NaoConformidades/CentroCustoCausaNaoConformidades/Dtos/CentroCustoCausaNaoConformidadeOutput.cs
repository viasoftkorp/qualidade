using System;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;

public class CentroCustoCausaNaoConformidadeOutput : ICentroCustoCausaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdCentroCusto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdCausaNaoConformidade { get; set; }
    public Guid CompanyId { get; set; }

    public CentroCustoCausaNaoConformidadeOutput()
    {

    }

    public CentroCustoCausaNaoConformidadeOutput(CentroCustoCausaNaoConformidade centroCustoCausaNaoConformidade)
    {
        Id = centroCustoCausaNaoConformidade.Id;
        IdCentroCusto = centroCustoCausaNaoConformidade.IdCentroCusto;
        IdNaoConformidade = centroCustoCausaNaoConformidade.IdNaoConformidade;
        IdCausaNaoConformidade = centroCustoCausaNaoConformidade.IdCausaNaoConformidade;
    }
}
