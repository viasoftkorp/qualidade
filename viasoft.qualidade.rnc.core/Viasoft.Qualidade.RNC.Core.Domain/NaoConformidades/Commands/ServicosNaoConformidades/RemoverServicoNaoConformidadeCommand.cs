using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;

public class RemoverServicoNaoConformidadeCommand
{
    public RemoverServicoNaoConformidadeCommand()
    {
    }
    public Guid Id { get; set; }
    public RemoverServicoNaoConformidadeCommand(Guid id)
    {
        Id = id;
    }
}