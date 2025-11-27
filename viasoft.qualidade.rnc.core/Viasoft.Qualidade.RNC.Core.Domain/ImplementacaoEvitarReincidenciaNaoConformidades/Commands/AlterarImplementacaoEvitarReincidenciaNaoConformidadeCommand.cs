using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;

public class AlterarImplementacaoEvitarReincidenciaNaoConformidadeCommand
{
    public ImplementacaoEvitarReincidenciaNaoConformidadeModel ImplementacaoEvitarReincidenciaNaoConformidade { get; set; }
    public AlterarImplementacaoEvitarReincidenciaNaoConformidadeCommand()
    {
    }

    public AlterarImplementacaoEvitarReincidenciaNaoConformidadeCommand(IImplementacaoEvitarReincidenciaNaoConformidadeModel model)
    {
        ImplementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaNaoConformidadeModel(model);
    }
}