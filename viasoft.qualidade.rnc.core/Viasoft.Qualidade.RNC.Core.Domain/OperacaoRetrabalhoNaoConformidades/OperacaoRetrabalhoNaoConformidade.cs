using System;
using System.Collections.Generic;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;

namespace Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;

public class OperacaoRetrabalhoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid IdNaoConformidade { get; set; }
    
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public decimal Quantidade { get; set; }
    public string NumeroOperacaoARetrabalhar { get; set; }
    public NaoConformidade NaoConformidade { get; set; }

    public List<Operacao> Operacoes { get; set; } = new List<Operacao>();

}