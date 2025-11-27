using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;

public class InserirNaoConformidadeCommand
{
    public NaoConformidadeModel NaoConformidade { get; set; }
    public InserirNaoConformidadeCommand()
    {
    }

    public InserirNaoConformidadeCommand(INaoConformidadeModel model)
    {
        NaoConformidade = new NaoConformidadeModel(model);
    }
}