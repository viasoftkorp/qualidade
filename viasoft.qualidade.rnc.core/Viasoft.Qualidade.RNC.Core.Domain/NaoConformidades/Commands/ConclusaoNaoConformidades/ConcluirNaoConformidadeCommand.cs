using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ConclusaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ConclusaoNaoConformidades;

public class ConcluirNaoConformidadeCommand
{
    public ConclusaoNaoConformidadeModel ConclusaoNaoConformidade { get; set; }
    public ConcluirNaoConformidadeCommand()
    {
    }

    public ConcluirNaoConformidadeCommand(IConclusaoNaoConformidadeModel model)
    {
        ConclusaoNaoConformidade = new ConclusaoNaoConformidadeModel(model);
    }
}