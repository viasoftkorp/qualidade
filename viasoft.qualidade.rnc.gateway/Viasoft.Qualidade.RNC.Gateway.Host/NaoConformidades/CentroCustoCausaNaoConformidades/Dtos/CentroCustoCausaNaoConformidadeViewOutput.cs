using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;

public class CentroCustoCausaNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdCausaNaoConformidade { get; set; }
    public string CodigoCentroCusto { get; set; }
    public string DescricaoCentroCusto { get; set; }
    public bool IsCentroCustoSintetico { get; set; }
}
