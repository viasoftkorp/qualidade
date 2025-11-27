using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.DefeitosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.DefeitoNaoConformidadeInserido")]
public class DefeitoNaoConformidadeInserido : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }
    public InserirDefeitoCommand Command { get; set; }

    public DefeitoNaoConformidadeInserido()
    {
    }

    public DefeitoNaoConformidadeInserido(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}