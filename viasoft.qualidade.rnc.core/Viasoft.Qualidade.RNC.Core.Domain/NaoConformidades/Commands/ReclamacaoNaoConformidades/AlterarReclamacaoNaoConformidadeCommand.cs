using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ReclamacoesNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;

public class AlterarReclamacaoNaoConformidadeCommand 
{
    public ReclamacaoNaoConformidadeModel ReclamacaoNaoConformidade { get; set; }
    public AlterarReclamacaoNaoConformidadeCommand()
    {
    }

    public AlterarReclamacaoNaoConformidadeCommand(IReclamacaoNaoConformidadeModel model)
    {
        ReclamacaoNaoConformidade = new ReclamacaoNaoConformidadeModel(model);
    }
}