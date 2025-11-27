using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Events;

[Endpoint("Viasoft.Qualidade.RNC.DefeitoUpdated")]
public class DefeitoUpdated : IEvent
{
    public DefeitoUpdated(Defeito defeito, Guid id, DateTime asOfDate, Guid tenantId,
        Guid environmentId)
    {
        IdDefeito = defeito.Id;
        IdSolucao = defeito.IdSolucao;
        IdCausa = defeito.IdCausa;
        Descricao = defeito.Descricao;
        Detalhamento = defeito.Detalhamento;
        Id = id;
        AsOfDate = asOfDate;
        TenantId = tenantId;
        EnvironmentId = environmentId;
    }

    public DefeitoUpdated()
    {
    }

    public Guid Id { get; set; }
    public DateTime AsOfDate { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid? IdSolucao { get; set; }
    public Guid? IdCausa { get; set; }
    public Guid IdDefeito { get; set; }

    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
}