using System;
using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class SolucaoDataSource
{
    public List<RelatorioSolucaoNaoConformidade> SolucoesNaoConformidade { get; set; }
}

public class RelatorioSolucaoNaoConformidade
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdSolucao { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public bool SolucaoImediata { get; set; }
    public string DataAnalise { get; set; }
    public string DataPrevistaImplantacao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public decimal CustoEstimado { get; set; }
    public string NovaData { get; set; }
    public string DataVerificacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public string Responsavel { get; set; }
    public string Auditor { get; set; }                                             

    public RelatorioSolucaoNaoConformidade()
    {
    }

    public RelatorioSolucaoNaoConformidade(SolucaoNaoConformidadeViewOutput solucaoNaoConformidade)
    {
        Id = solucaoNaoConformidade.Id;
        IdNaoConformidade = solucaoNaoConformidade.IdNaoConformidade;
        IdDefeitoNaoConformidade = solucaoNaoConformidade.IdDefeitoNaoConformidade;
        IdSolucao = solucaoNaoConformidade.IdSolucao;
        Codigo = solucaoNaoConformidade.Codigo;
        Descricao = solucaoNaoConformidade.Descricao;
        Detalhamento = solucaoNaoConformidade.Detalhamento;
        SolucaoImediata = solucaoNaoConformidade.SolucaoImediata;
        DataAnalise = solucaoNaoConformidade.DataAnalise.ToString();
        DataPrevistaImplantacao = solucaoNaoConformidade.DataPrevistaImplantacao.ToString();
        IdResponsavel = solucaoNaoConformidade.IdResponsavel;
        CustoEstimado = solucaoNaoConformidade.CustoEstimado;
        NovaData = solucaoNaoConformidade.NovaData.ToString();
        DataVerificacao = solucaoNaoConformidade.DataVerificacao.ToString();
        IdAuditor = solucaoNaoConformidade.IdAuditor;
        Responsavel = solucaoNaoConformidade.Responsavel;
        Auditor = solucaoNaoConformidade.Auditor;
    }
}