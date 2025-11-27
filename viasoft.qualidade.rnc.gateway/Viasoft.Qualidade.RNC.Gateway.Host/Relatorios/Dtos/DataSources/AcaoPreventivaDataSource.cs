using System;
using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class AcaoPreventivaDataSource
{
    public List<RelatorioAcaoPreventivaNaoConformidade> AcoesPreventivasNaoConformidade { get; set; }
}

public class RelatorioAcaoPreventivaNaoConformidade
{
    public Guid Id { get; set; }                                    
    public Guid IdAcaoPreventiva { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Acao { get; set; }
    public string Responsavel { get; set; }
    public string Auditor { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public Guid? IdResponsavel { get; set; }
    public string DataAnalise { get; set; }
    public string DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public bool Implementada { get; set; }
    public string DataVerificacao { get; set; }
    public string NovaData { get; set; }                                             

    public RelatorioAcaoPreventivaNaoConformidade()
    {
    }

    public RelatorioAcaoPreventivaNaoConformidade(AcaoPreventivaNaoConformidadeViewOutput acaoPreventivaNaoConformidade)
    {
        Id = acaoPreventivaNaoConformidade.Id;
        IdAcaoPreventiva = acaoPreventivaNaoConformidade.IdAcaoPreventiva;
        IdNaoConformidade = acaoPreventivaNaoConformidade.IdNaoConformidade;
        IdDefeitoNaoConformidade = acaoPreventivaNaoConformidade.IdDefeitoNaoConformidade;
        Acao = acaoPreventivaNaoConformidade.Acao;
        Responsavel = acaoPreventivaNaoConformidade.Responsavel;
        Auditor = acaoPreventivaNaoConformidade.Auditor;
        Codigo = acaoPreventivaNaoConformidade.Codigo;
        Descricao = acaoPreventivaNaoConformidade.Descricao;
        Detalhamento = acaoPreventivaNaoConformidade.Detalhamento;
        IdResponsavel = acaoPreventivaNaoConformidade.IdResponsavel;
        DataAnalise = acaoPreventivaNaoConformidade.DataAnalise.ToString();
        DataPrevistaImplantacao = acaoPreventivaNaoConformidade.DataPrevistaImplantacao.ToString();
        IdAuditor = acaoPreventivaNaoConformidade.IdAuditor;
        Implementada = acaoPreventivaNaoConformidade.Implementada;
        DataVerificacao = acaoPreventivaNaoConformidade.DataVerificacao.ToString();
        NovaData = acaoPreventivaNaoConformidade.NovaData.ToString();
    }
}