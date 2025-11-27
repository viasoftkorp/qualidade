using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;

public class RemoverAcaoPreventivaCommand
{
    public RemoverAcaoPreventivaCommand()
    {
    }
    public Guid Id { get; set; }
    public RemoverAcaoPreventivaCommand(Guid id)
    {
        Id = id;
    }
}