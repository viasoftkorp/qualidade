using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

public class SolucaoNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdSolucao { get; set; }
    public int Codigo { get; set; }
    public string Descricao{get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public string Responsavel { get; set; }
    public string Auditor { get; set; }
    public bool SolucaoImediata { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public decimal CustoEstimado { get; set; }
    public DateTime? NovaData { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public Guid? IdAuditor { get; set; }

    public SolucaoNaoConformidadeViewOutput()
    {
    }
    
    public SolucaoNaoConformidadeViewOutput(SolucaoNaoConformidade solucaoNaoConformidade, Solucao solucao, Usuario usuario)
    {
        Id = solucaoNaoConformidade.Id;
        IdNaoConformidade = solucaoNaoConformidade.IdNaoConformidade;
        IdDefeitoNaoConformidade = solucaoNaoConformidade.IdDefeitoNaoConformidade;
        SolucaoImediata = solucaoNaoConformidade.SolucaoImediata;
        DataAnalise = solucaoNaoConformidade.DataAnalise;
        DataPrevistaImplantacao = solucaoNaoConformidade.DataPrevistaImplantacao;
        IdResponsavel = solucaoNaoConformidade.IdResponsavel;
        Responsavel = $"{usuario.Nome} {usuario.Sobrenome}"; 
        Auditor = $"{usuario.Nome} {usuario.Sobrenome}";
        CustoEstimado = solucaoNaoConformidade.CustoEstimado;
        NovaData = solucaoNaoConformidade.NovaData;
        DataVerificacao = solucaoNaoConformidade.DataVerificacao;
        IdSolucao = solucaoNaoConformidade.IdSolucao;
        Detalhamento = solucaoNaoConformidade.Detalhamento;
        Codigo = solucao.Codigo;
        Descricao = solucao.Descricao;
        IdAuditor = solucaoNaoConformidade.IdAuditor;
    }
}