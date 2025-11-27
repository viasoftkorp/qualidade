using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Events;

[Endpoint("Viasoft.Qualidade.RNC.Core.OrdemRetrabalhoNaoConformidadeInserida")]
public class OrdemRetrabalhoNaoConformidadeInserida : IEvent
{
    public OrdemRetrabalhoNaoConformidade OrdemRetrabalhoNaoConformidade { get; set; }
}