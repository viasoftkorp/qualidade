using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ProdutosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ProdutoNaoConformidadeRemovido")]
public class ProdutoNaoConformidadeRemovido : BaseEvent, IEvent
{
    public Guid IdProdutoSolucao { get; set; }

    public ProdutoNaoConformidadeRemovido()
    {
    }

    public ProdutoNaoConformidadeRemovido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}