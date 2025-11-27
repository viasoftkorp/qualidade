using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;

public class AlterarCausaCommand
{
    public CausaNaoConformidadeModel CausaNaoConformidade { get; set; }
    public AlterarCausaCommand()
    {
    }

    public AlterarCausaCommand(ICausaNaoConformidadeModel model)
    {
        CausaNaoConformidade = new CausaNaoConformidadeModel(model);
    }
}