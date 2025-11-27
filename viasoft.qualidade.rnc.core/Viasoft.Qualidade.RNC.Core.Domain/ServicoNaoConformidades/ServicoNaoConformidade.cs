using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;

public class ServicoNaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public Guid? IdProduto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    public int Horas { get; set; }
    public int Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    public string Detalhamento { get; set; }
    public string OperacaoEngenharia { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }
    public bool ControlarApontamento { get; set; }

    public ServicoNaoConformidade()
    {
    }

    public ServicoNaoConformidade(ServicoNaoConformidade servico)
    {
        Id = servico.Id;
        IdProduto = servico.IdProduto;
        IdNaoConformidade = servico.IdNaoConformidade;
        Quantidade = servico.Quantidade;
        Horas = servico.Horas;
        Minutos = servico.Minutos;
        IdRecurso = servico.IdRecurso;
        Detalhamento = servico.Detalhamento;
        OperacaoEngenharia = servico.OperacaoEngenharia;
        ControlarApontamento = servico.ControlarApontamento;
    }
}