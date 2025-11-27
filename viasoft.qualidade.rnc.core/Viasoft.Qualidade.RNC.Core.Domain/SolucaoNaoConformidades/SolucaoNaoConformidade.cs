using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;

public class SolucaoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdSolucao { get; set; }
    public bool SolucaoImediata { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public decimal CustoEstimado { get; set; }
    public DateTime? NovaData { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public string Detalhamento { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }

    public SolucaoNaoConformidade()
    {
    }

    public SolucaoNaoConformidade(SolucaoNaoConformidade solucao)
    {
        Id = solucao.Id;
        IdNaoConformidade = solucao.IdNaoConformidade;
        IdDefeitoNaoConformidade = solucao.IdDefeitoNaoConformidade;
        IdSolucao = solucao.IdSolucao;
        SolucaoImediata = solucao.SolucaoImediata;
        DataAnalise = solucao.DataAnalise;
        DataPrevistaImplantacao = solucao.DataPrevistaImplantacao;
        IdResponsavel = solucao.IdResponsavel;
        CustoEstimado = solucao.CustoEstimado;
        NovaData = solucao.NovaData;
        DataVerificacao = solucao.DataVerificacao;
        IdAuditor = solucao.IdAuditor;
        Detalhamento = solucao.Detalhamento;
    }
}