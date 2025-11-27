using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;

public class ImplementacaoEvitarReincidenciaNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    [IsArrayOfBytes]
    public string Descricao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }
    public bool AcaoImplementada { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }
    
    public ImplementacaoEvitarReincidenciaNaoConformidade()
    {
    }

    public ImplementacaoEvitarReincidenciaNaoConformidade(IImplementacaoEvitarReincidenciaNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        IdDefeitoNaoConformidade = model.IdDefeitoNaoConformidade;
        Descricao = model.Descricao;
        IdResponsavel = model.IdResponsavel;
        DataAnalise = model.DataAnalise;
        DataPrevistaImplantacao = model.DataPrevistaImplantacao;
        IdAuditor = model.IdAuditor;
        DataVerificacao = model.DataVerificacao;
        NovaData = model.NovaData;
        AcaoImplementada = model.AcaoImplementada;
    }
}
