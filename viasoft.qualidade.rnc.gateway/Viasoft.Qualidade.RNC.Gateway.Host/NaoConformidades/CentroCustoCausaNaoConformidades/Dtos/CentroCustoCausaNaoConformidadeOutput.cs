using System;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;

public class CentroCustoCausaNaoConformidadeOutput : EntityDto
{
    public Guid IdCentroCusto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdCausaNaoConformidade { get; set; }
}
