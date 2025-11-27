using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.AcoesPreventivasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;

public class InserirAcaoPreventivaCommand
{
    public AcaoPreventivaNaoConformidadeModel AcaoPreventivaNaoConformidade { get; set; }
    public InserirAcaoPreventivaCommand()
    {
    }

    public InserirAcaoPreventivaCommand(IAcaoPreventivaNaoConformidadeModel model)
    {
        AcaoPreventivaNaoConformidade = new AcaoPreventivaNaoConformidadeModel(model);
    }
}