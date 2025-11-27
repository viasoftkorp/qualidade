using System;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Commands;

public class RemoverCentroCustoCausaNaoConformidadeCommand
{
    public Guid Id { get; set; }
    public RemoverCentroCustoCausaNaoConformidadeCommand()
    {
    }

    public RemoverCentroCustoCausaNaoConformidadeCommand(Guid id)
    {
        Id = id;
    }
}
