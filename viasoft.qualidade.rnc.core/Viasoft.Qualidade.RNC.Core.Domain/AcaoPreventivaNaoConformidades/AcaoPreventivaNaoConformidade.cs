using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;

public class AcaoPreventivaNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    public Guid IdAcaoPreventiva { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Acao { get; set; }
    public string Detalhamento { get; set; }
    public Guid? IdResponsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public bool Implementada { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }

    public AcaoPreventivaNaoConformidade()
    {
    }

    public AcaoPreventivaNaoConformidade(AcaoPreventivaNaoConformidade acao)
    {
        Id = acao.Id;
        IdNaoConformidade = acao.IdNaoConformidade;
        IdAcaoPreventiva = acao.IdAcaoPreventiva;
        IdDefeitoNaoConformidade = acao.IdDefeitoNaoConformidade;
        Acao = acao.Acao;
        Detalhamento = acao.Detalhamento;
        IdResponsavel = acao.IdResponsavel;
        DataAnalise = acao.DataAnalise;
        DataPrevistaImplantacao = acao.DataPrevistaImplantacao;
        IdAuditor = acao.IdAuditor;
        Implementada = acao.Implementada;
        DataVerificacao = acao.DataVerificacao;
        NovaData = acao.NovaData;
    }
}