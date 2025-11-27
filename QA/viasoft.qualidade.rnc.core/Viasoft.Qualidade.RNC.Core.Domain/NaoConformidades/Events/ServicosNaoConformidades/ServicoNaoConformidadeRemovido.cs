using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ServicosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ServicoSolucaoNaoConformidadeRemovido")]
public class ServicoNaoConformidadeRemovido : BaseEvent, IEvent
{
    public Guid IdServicoSolucao { get; set; }

    public ServicoNaoConformidadeRemovido()
    {
    }

    public ServicoNaoConformidadeRemovido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}