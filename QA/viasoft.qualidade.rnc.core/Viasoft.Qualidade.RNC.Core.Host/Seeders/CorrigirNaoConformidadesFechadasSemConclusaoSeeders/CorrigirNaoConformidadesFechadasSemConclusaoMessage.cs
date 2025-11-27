using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNaoConformidadesFechadasSemConclusaoSeeders;

[Endpoint("Viasoft.Qualidade.RNC.Core.CorrigirNaoConformidadesFechadasSemConclusao", "Viasoft.Qualidade.RNC.Core")]

public class CorrigirNaoConformidadesFechadasSemConclusaoCommand : ICommand
{
    
}

[Endpoint("Viasoft.Qualidade.RNC.Core.CorrigirNaoConformidadesFechadasSemConclusao")]
public class CorrigirNaoConformidadesFechadasSemConclusaoMessage: IMessage
{
    
}