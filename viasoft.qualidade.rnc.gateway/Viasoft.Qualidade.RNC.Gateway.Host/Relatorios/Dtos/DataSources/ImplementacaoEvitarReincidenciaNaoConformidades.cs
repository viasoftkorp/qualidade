using System;
using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class ImplementacaoEvitarReincidenciaDataSource
{
    public List<RelatorioImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidenciaNaoConformidade { get; set; }
}

public class RelatorioImplementacaoEvitarReincidenciaNaoConformidade
{
    public Guid Id { get; set; }                        
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Descricao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public string Responsavel { get; set; }
    public string DataAnalise { get; set; }
    public string DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public string Auditor { get; set; }    
    public string DataVerificacao { get; set; }
    public string NovaData { get; set; }
    public bool AcaoImplementada { get; set; }
    public Guid CompanyId { get; set; }

    public RelatorioImplementacaoEvitarReincidenciaNaoConformidade()
    {
    }

    public RelatorioImplementacaoEvitarReincidenciaNaoConformidade(ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput implementacaoEvitarReincidenciaNaoConformidade)
    {
        Id = implementacaoEvitarReincidenciaNaoConformidade.Id;
        IdNaoConformidade = implementacaoEvitarReincidenciaNaoConformidade.IdNaoConformidade;
        IdDefeitoNaoConformidade = implementacaoEvitarReincidenciaNaoConformidade.IdDefeitoNaoConformidade;
        Descricao = implementacaoEvitarReincidenciaNaoConformidade.Descricao;
        IdResponsavel = implementacaoEvitarReincidenciaNaoConformidade.IdResponsavel;
        Responsavel = implementacaoEvitarReincidenciaNaoConformidade.Responsavel;
        DataAnalise = implementacaoEvitarReincidenciaNaoConformidade.DataAnalise.ToString();
        DataPrevistaImplantacao = implementacaoEvitarReincidenciaNaoConformidade.DataPrevistaImplantacao.ToString();
        IdAuditor = implementacaoEvitarReincidenciaNaoConformidade.IdAuditor;
        Auditor = implementacaoEvitarReincidenciaNaoConformidade.Auditor;
        DataVerificacao = implementacaoEvitarReincidenciaNaoConformidade.DataVerificacao.ToString();
        NovaData = implementacaoEvitarReincidenciaNaoConformidade.NovaData.ToString();
        AcaoImplementada = implementacaoEvitarReincidenciaNaoConformidade.AcaoImplementada;
        CompanyId = implementacaoEvitarReincidenciaNaoConformidade.CompanyId;
    }
}