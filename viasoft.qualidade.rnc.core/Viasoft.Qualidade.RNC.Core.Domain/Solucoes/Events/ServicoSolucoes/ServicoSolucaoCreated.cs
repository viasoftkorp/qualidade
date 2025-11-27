using System;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Attributes;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;

[Endpoint("Viasoft.Qualidade.RNC.ServicoSolucaoCreated")]
public class ServicoSolucaoCreated : IEvent
{
    public ServicoSolucaoCreated(Solucoes.ServicoSolucao servicoSolucao, Guid id, DateTime asOfDate, Guid tenantId,
        Guid environmentId)
    {
        IdSolucao = servicoSolucao.IdSolucao;
        IdProduto = servicoSolucao.IdProduto;
        IdServicoSolucao = servicoSolucao.Id;
        Quantidade = servicoSolucao.Quantidade;
        Id = id;
        AsOfDate = asOfDate;
        TenantId = tenantId;
        EnvironmentId = environmentId;
        IdRecurso = servicoSolucao.IdRecurso;
        OperacaoEngenharia = servicoSolucao.OperacaoEngenharia;
        Horas = servicoSolucao.Horas;
        Minutos = servicoSolucao.Minutos;
    }

    public ServicoSolucaoCreated()
    {
    }

    public Guid Id { get; set; }
    public DateTime AsOfDate { get; set; }
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid? IdProduto { get; set; }
    public Guid IdServicoSolucao { get; set; }
    public Guid IdRecurso { get; set; }
    public string OperacaoEngenharia { get; set; }
    public int? Horas { get; set; }
    public int? Minutos { get; set; }
    
    [StrictRequired] 
    public int Quantidade { get; set; }
}