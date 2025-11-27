using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;

public class InserirServicoNaoConformidadeCommand
{
    public ServicoNaoConformidadeModel ServicoNaoConformidade { get; set; }
    public InserirServicoNaoConformidadeCommand()
    {
    }

    public InserirServicoNaoConformidadeCommand(IServicoNaoConformidadeModel model)
    {
        ServicoNaoConformidade = new ServicoNaoConformidadeModel(model);
    }
}