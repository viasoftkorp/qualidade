using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.SolucoesNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades
{
    public class AlterarSolucaoCommand
    {
        public SolucaoNaoConformidadeModel SolucaoNaoConformidade { get; set; }
        public AlterarSolucaoCommand()
        {
        }

        public AlterarSolucaoCommand(ISolucaoNaoConformidadeModel model)
        {
            SolucaoNaoConformidade = new SolucaoNaoConformidadeModel(model);
        }
    }
}