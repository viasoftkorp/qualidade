using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Models;

public interface ICentroCustoCausaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdCentroCusto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdCausaNaoConformidade { get; set; }
    public Guid CompanyId { get; set; }
}
