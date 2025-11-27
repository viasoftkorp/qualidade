using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades;

public class RemoverSolucaoCommand
{
    public RemoverSolucaoCommand()
    {
    }
    public Guid Id { get; set; }
    public RemoverSolucaoCommand(Guid id)
    {
        Id = id;
    }
}