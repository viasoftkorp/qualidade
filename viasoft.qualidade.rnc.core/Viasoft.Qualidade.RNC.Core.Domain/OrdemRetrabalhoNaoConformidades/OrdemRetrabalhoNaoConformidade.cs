using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;

public class OrdemRetrabalhoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public int NumeroOdfRetrabalho { get; set; }
    public decimal Quantidade { get; set; }
    public Guid IdLocalOrigem { get; set; }
    public Guid? IdEstoqueLocalDestino { get; set; }
    public Guid IdLocalDestino { get; set; }
    public string MovimentacaoEstoqueMensagemRetorno { get; set; }
    public string CodigoArmazem { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }

    public OrdemRetrabalhoNaoConformidade()
    {
        
    }
    public OrdemRetrabalhoNaoConformidade(OrdemRetrabalhoNaoConformidadeModel model)
    {

        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        NumeroOdfRetrabalho = model.NumeroOdfRetrabalho;
        Quantidade = model.Quantidade;
        MovimentacaoEstoqueMensagemRetorno = model.MovimentacaoEstoqueMensagemRetorno;
    }

    public void ChangeStatus(StatusProducaoRetrabalho novoStatus)
    {
        Status = novoStatus;
    }
}