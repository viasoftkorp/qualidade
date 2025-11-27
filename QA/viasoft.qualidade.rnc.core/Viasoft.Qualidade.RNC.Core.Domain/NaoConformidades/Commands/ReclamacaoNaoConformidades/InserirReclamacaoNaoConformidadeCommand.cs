using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ReclamacoesNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;

public class InserirReclamacaoNaoConformidadeCommand
{
    public ReclamacaoNaoConformidadeModel ReclamacaoNaoConformidade { get; set; }
    public InserirReclamacaoNaoConformidadeCommand()
    {
    }

    public InserirReclamacaoNaoConformidadeCommand(IReclamacaoNaoConformidadeModel model)
    {
        ReclamacaoNaoConformidade = new ReclamacaoNaoConformidadeModel(model);
    }
}