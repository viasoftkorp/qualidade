using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.AcoesPreventivasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;

public class AlterarAcaoPreventivaCommand
{
    public AcaoPreventivaNaoConformidadeModel AcaoPreventivaNaoConformidade { get; set; }
    public AlterarAcaoPreventivaCommand()
    {
    }

    public AlterarAcaoPreventivaCommand(IAcaoPreventivaNaoConformidadeModel model)
    {
        AcaoPreventivaNaoConformidade = new AcaoPreventivaNaoConformidadeModel(model);
    }
}