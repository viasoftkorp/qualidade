using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ProdutosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ProdutoNaoConformidadeInserido")]
public class ProdutoNaoConformidadeInserido : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }
    public InserirProdutoNaoConformidadeCommand Command { get; set; }

    public ProdutoNaoConformidadeInserido()
    {
    }

    public ProdutoNaoConformidadeInserido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}