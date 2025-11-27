using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;

public class RemoverDefeitoCommand
{
    public RemoverDefeitoCommand()
    {
    }
    public Guid Id { get; set; }
    public RemoverDefeitoCommand(Guid id)
    {
        Id = id;
    }
}