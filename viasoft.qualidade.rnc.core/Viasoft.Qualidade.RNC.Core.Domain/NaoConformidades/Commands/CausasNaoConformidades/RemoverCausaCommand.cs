using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;

public class RemoverCausaCommand
{
    public RemoverCausaCommand()
    {
    }
    public Guid Id { get; set; }
    public RemoverCausaCommand(Guid id)
    {
        Id = id;
    }
}