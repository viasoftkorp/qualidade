using System;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.DefeitosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;

public class AlterarDefeitoCommand
{
    public Guid IdDefeitoAnterior { get; set; }
    public DefeitoNaoConformidadeModel DefeitoNaoConformidade { get; set; }
    public AlterarDefeitoCommand()
    {
    }

    public AlterarDefeitoCommand(IDefeitoNaoConformidadeModel model)
    {
        DefeitoNaoConformidade = new DefeitoNaoConformidadeModel(model);
    }
}