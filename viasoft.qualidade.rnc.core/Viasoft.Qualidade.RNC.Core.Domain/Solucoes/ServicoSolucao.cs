using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes;

public class ServicoSolucao : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid? IdProduto { get; set; }
    public int Quantidade { get; set; }
    public int Horas { get; set; }
    public int Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    [IsArrayOfBytes]
    public string OperacaoEngenharia { get; set; }

    public ServicoSolucao()
    {
    }

    public ServicoSolucao(ServicoSolucaoModel servicoSolucao)
    {
        Id = servicoSolucao.Id;
        IdProduto = servicoSolucao.IdProduto;
        IdSolucao = servicoSolucao.IdSolucao;
        Quantidade = servicoSolucao.Quantidade;
        Horas = servicoSolucao.Horas;
        Minutos = servicoSolucao.Minutos;
        IdRecurso = servicoSolucao.IdRecurso;
        OperacaoEngenharia = servicoSolucao.OperacaoEngenharia;
    } 
    public ServicoSolucao(ServicoSolucao servicoSolucao)
    {
        Id = servicoSolucao.Id;
        IdProduto = servicoSolucao.IdProduto;
        IdSolucao = servicoSolucao.IdSolucao;
        Quantidade = servicoSolucao.Quantidade;
        Horas = servicoSolucao.Horas;
        Minutos = servicoSolucao.Minutos;
        IdRecurso = servicoSolucao.IdRecurso;
        OperacaoEngenharia = servicoSolucao.OperacaoEngenharia;
    }

    public void Update(ServicoSolucaoModel servicoSolucao)
    {
        Quantidade = servicoSolucao.Quantidade;
        Horas = servicoSolucao.Horas;
        Minutos = servicoSolucao.Minutos;
        IdProduto = servicoSolucao.IdProduto;
        IdRecurso = servicoSolucao.IdRecurso;
        OperacaoEngenharia = servicoSolucao.OperacaoEngenharia;
    }
}