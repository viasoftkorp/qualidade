using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;

public class RemoverNaoConformidadeCommand
{
    public RemoverNaoConformidadeCommand()
    {
    }

    public Guid Id { get; set; }

    public RemoverNaoConformidadeCommand(Guid id)
    {
        Id = id;
    }
}