using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;

public class AlterarNaoConformidadeCommand
{
    public NaoConformidadeModel NaoConformidade { get; set; }
    public AlterarNaoConformidadeCommand()
    {
    }

    public AlterarNaoConformidadeCommand(INaoConformidadeModel model)
    {
        NaoConformidade = new NaoConformidadeModel(model);
    }
}