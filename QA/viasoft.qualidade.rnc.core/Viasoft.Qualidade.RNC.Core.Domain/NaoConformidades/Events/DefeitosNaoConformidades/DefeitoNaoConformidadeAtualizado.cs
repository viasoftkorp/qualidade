using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.DefeitosNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.DefeitoNaoConformidadeAtualizado")]
public class DefeitoNaoConformidadeAtualizado : BaseEvent, IEvent
{
    public AlterarDefeitoCommand Command { get; set; }
    public Guid IdDefeitoAnterior { get; set; }
    public DefeitoNaoConformidadeAtualizado()
    {
    }

    public DefeitoNaoConformidadeAtualizado(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}