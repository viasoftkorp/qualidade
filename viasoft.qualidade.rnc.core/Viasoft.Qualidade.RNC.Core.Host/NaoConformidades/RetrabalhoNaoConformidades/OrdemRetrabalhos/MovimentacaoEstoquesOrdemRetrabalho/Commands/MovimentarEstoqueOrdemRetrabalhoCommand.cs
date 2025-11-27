using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Commands;

[Endpoint("Viasoft.Qualidade.RNC.Core.MovimentarEstoqueItem")]
public class MovimentarEstoqueOrdemRetrabalhoCommand : ICommand
{
    public Guid IdNaoConformidade { get; set; }
    public bool IsEstorno { get; set; }
}
[Endpoint("Viasoft.Qualidade.RNC.Core.MovimentarEstoqueItem")]
public class MovimentarEstoqueItemMessage : IMessage
{
    public Guid IdNaoConformidade { get; set; }
    public bool IsEstorno { get; set; }
}