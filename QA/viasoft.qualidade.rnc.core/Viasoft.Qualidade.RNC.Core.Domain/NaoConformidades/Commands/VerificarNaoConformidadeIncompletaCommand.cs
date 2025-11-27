using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;

[Endpoint("Viasoft.Qualidade.RNC.VerificarNaoConformidadeIncompleta")]
public class VerificarNaoConformidadeIncompletaCommand : ICommand
{
    public Guid IdNaoConformidade { get; set; }
}

[Endpoint("Viasoft.Qualidade.RNC.VerificarNaoConformidadeIncompleta")]
public class VerificarNaoConformidadeIncompletaMessage : IMessage
{
    public Guid IdNaoConformidade { get; set; }

}