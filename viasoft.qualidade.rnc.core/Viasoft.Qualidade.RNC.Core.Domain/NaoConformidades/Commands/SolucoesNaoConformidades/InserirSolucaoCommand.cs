using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.SolucoesNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades;

public class InserirSolucaoCommand
{
    public SolucaoNaoConformidadeModel SolucaoNaoConformidade { get; set; }
    public InserirSolucaoCommand()
    {
    }

    public InserirSolucaoCommand(ISolucaoNaoConformidadeModel model)
    {
        SolucaoNaoConformidade = new SolucaoNaoConformidadeModel(model);
    }
}