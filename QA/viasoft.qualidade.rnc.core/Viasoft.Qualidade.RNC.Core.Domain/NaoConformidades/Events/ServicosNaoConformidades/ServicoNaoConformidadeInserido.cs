using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ServicosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.ServicoSolucaoNaoConformidadeInserido")]
public class ServicoNaoConformidadeInserido : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }
    public InserirServicoNaoConformidadeCommand ServicoNaoConformidade { get; set; }

    public ServicoNaoConformidadeInserido()
    {
    }

    public ServicoNaoConformidadeInserido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}