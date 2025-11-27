using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

public class ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput :
        IImplementacaoEvitarReincidenciaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    [IsArrayOfBytes]
    public string Descricao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public string Responsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public string Auditor { get; set; }    
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }
    public bool AcaoImplementada { get; set; }
    public Guid CompanyId { get; set; }

    public ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput()
    {
        
    }
    public ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput(
        ImplementacaoEvitarReincidenciaNaoConformidade entity)
    {
        Id = entity.Id;
        IdNaoConformidade = entity.IdNaoConformidade;
        IdDefeitoNaoConformidade = entity.IdDefeitoNaoConformidade;
        Descricao = entity.Descricao;
        IdResponsavel = entity.IdResponsavel;
        DataAnalise = entity.DataAnalise;
        DataPrevistaImplantacao = entity.DataPrevistaImplantacao;
        IdAuditor = entity.IdAuditor;
        DataVerificacao = entity.DataVerificacao;
        NovaData = entity.NovaData;
        AcaoImplementada = entity.AcaoImplementada;
    }
}