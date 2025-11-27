using System;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;

public class InserirCausaCommand
{
    public CausaNaoConformidadeModel CausaNaoConformidade { get; set; }
    public InserirCausaCommand()
    {
    }

    public InserirCausaCommand(ICausaNaoConformidadeModel model)
    {
        CausaNaoConformidade = new CausaNaoConformidadeModel(model)
        {
            Id = Guid.NewGuid()
        };
    }
}
