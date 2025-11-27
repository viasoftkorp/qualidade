using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Models;

public class ImplementacaoEvitarReincidenciaNaoConformidadeModel : IImplementacaoEvitarReincidenciaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Descricao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }
    public bool AcaoImplementada { get; set; }
    public Guid CompanyId { get; set; }
    
    public ImplementacaoEvitarReincidenciaNaoConformidadeModel()
    {
    }

    public ImplementacaoEvitarReincidenciaNaoConformidadeModel(IImplementacaoEvitarReincidenciaNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        Descricao = model.Descricao;
        IdResponsavel = model.IdResponsavel;
        DataAnalise = model.DataAnalise;
        DataPrevistaImplantacao = model.DataPrevistaImplantacao;
        IdAuditor = model.IdAuditor;
        DataVerificacao = model.DataVerificacao;
        NovaData = model.NovaData;
        AcaoImplementada = model.AcaoImplementada;
        CompanyId = model.CompanyId;
        IdDefeitoNaoConformidade = model.IdDefeitoNaoConformidade;
    }
}