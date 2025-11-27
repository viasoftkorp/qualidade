using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ProdutosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ProdutoNaoConformidadeAtualizado")]
public class ProdutoNaoConformidadeAtualizado : BaseEvent, IEvent
{
    public AlterarProdutoNaoConformidadeCommand Command { get; set; }
    
    public ProdutoNaoConformidadeAtualizado()
    {
    }

    public ProdutoNaoConformidadeAtualizado(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}