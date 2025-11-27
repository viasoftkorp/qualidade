using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;

public class RemoverImplementacaoEvitarReincidenciaNaoConformidadeCommand
{
    public Guid Id { get; set; }

    public RemoverImplementacaoEvitarReincidenciaNaoConformidadeCommand()
    {
    }
    public RemoverImplementacaoEvitarReincidenciaNaoConformidadeCommand(Guid id)
    {
        Id = id;
    }
}