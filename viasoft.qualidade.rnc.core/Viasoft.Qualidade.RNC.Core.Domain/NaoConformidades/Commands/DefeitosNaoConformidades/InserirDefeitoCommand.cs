using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.DefeitosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;

public class InserirDefeitoCommand
{
    public DefeitoNaoConformidadeModel DefeitoNaoConformidade { get; set; }
    public InserirDefeitoCommand()
    {
    }

    public InserirDefeitoCommand(IDefeitoNaoConformidadeModel model)
    {
        DefeitoNaoConformidade = new DefeitoNaoConformidadeModel(model);
    }
}