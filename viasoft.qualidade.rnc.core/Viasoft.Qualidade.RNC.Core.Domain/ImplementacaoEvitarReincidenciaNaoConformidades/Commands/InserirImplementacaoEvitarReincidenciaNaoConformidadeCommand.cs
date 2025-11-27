using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;

public class InserirImplementacaoEvitarReincidenciaNaoConformidadeCommand
{
    public ImplementacaoEvitarReincidenciaNaoConformidadeModel ImplementacaoEvitarReincidenciaNaoConformidade { get; set; } 
    public InserirImplementacaoEvitarReincidenciaNaoConformidadeCommand()
    {
    }

    public InserirImplementacaoEvitarReincidenciaNaoConformidadeCommand(IImplementacaoEvitarReincidenciaNaoConformidadeModel model)
    {
        ImplementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaNaoConformidadeModel(model);
    }
}