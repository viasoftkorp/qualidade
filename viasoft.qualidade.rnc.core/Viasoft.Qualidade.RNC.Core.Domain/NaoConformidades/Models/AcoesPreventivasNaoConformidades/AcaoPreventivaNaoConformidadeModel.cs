using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.AcoesPreventivasNaoConformidades;

public class AcaoPreventivaNaoConformidadeModel : IAcaoPreventivaNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdAcaoPreventiva { get; set; }
    public string Acao { get; set; }
    public string Detalhamento { get; set; }
    public Guid? IdResponsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public bool Implementada { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }
    public Guid CompanyId { get; set; }

    public AcaoPreventivaNaoConformidadeModel()
    {
    }

    public AcaoPreventivaNaoConformidadeModel(IAcaoPreventivaNaoConformidadeModel model)
    {
        Id = model.Id;
        IdDefeitoNaoConformidade = model.IdDefeitoNaoConformidade;
        IdAcaoPreventiva = model.IdAcaoPreventiva;
        IdNaoConformidade = model.IdNaoConformidade;
        Detalhamento = model.Detalhamento;
        Acao = model.Acao;
        IdResponsavel = model.IdResponsavel;
        DataAnalise = model.DataAnalise;
        DataPrevistaImplantacao = model.DataPrevistaImplantacao;
        IdAuditor = model.IdAuditor;
        Implementada = model.Implementada;
        DataVerificacao = model.DataVerificacao;
        NovaData = model.NovaData;
        CompanyId = model.CompanyId;
    }
}