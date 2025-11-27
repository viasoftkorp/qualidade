using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes;

public class Solucao : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public string Descricao { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public bool Imediata { get; set; }
    public int Codigo { get; set; } 
    public bool IsAtivo { get; set; }

    public Solucao()
    {
    }

    public Solucao(SolucaoModel solucao)
    {
        Id = solucao.Id;
        Descricao = solucao.Descricao;
        Codigo = solucao.Codigo;
        Detalhamento = solucao.Detalhamento;
        Imediata = solucao.Imediata;
        IsAtivo = solucao.IsAtivo;
    }

    public void Update(SolucaoModel solucao)
    {
        Descricao = solucao.Descricao;
        Detalhamento = solucao.Detalhamento;
        Imediata = solucao.Imediata;
    }
}