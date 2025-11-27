using System;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.SolucoesNaoConformidades;

[Endpoint("Viasoft.Qualidade.RNC.Core.SolucaoNaoConformidadeInserida")]
public class SolucaoNaoConformidadeInserida : BaseEvent, IEvent
{
    public Guid IdNaoConformidade { get; set; }
    public InserirSolucaoCommand Command { get; set; }

    public SolucaoNaoConformidadeInserida()
    {
    }

    public SolucaoNaoConformidadeInserida(IDateTimeProvider dateTimeProvider, Guid currentTenantId, Guid currentEnvironmentId)
        : base(dateTimeProvider, currentTenantId, currentEnvironmentId, null)
    {
    }
}