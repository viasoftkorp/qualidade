using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;

public class AlterarServicosNaoConformidadeCommand
{
    public ServicoNaoConformidadeModel ServicoNaoConformidade { get; set; }
    public AlterarServicosNaoConformidadeCommand()
    {
    }

    public AlterarServicosNaoConformidadeCommand(IServicoNaoConformidadeModel model)
    {
        ServicoNaoConformidade = new ServicoNaoConformidadeModel(model);
    }
}