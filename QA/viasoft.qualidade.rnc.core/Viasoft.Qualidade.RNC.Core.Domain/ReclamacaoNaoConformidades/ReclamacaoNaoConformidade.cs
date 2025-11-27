using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;

public class ReclamacaoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    public int Procedentes { get; set; }
    public int Improcedentes { get; set; }
    public decimal QuantidadeLote { get; set; }
    public decimal QuantidadeNaoConformidade { get; set; }
    public int DisposicaoProdutosAprovados { get; set; }
    public int DisposicaoProdutosConcessao { get; set; }
    public int Retrabalho { get; set; }
    public int Rejeitado { get; set; }
    public bool RetrabalhoComOnus { get; set; }
    public bool RetrabalhoSemOnus { get; set; }
    public bool DevolucaoFornecedor { get; set; }
    public bool Recodificar { get; set; }
    public bool Sucata { get; set; }
    public string Observacao { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }

    public ReclamacaoNaoConformidade()
    {
    }
}