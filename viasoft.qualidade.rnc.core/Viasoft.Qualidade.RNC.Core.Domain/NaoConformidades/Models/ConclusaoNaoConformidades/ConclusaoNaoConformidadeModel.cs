using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ConclusaoNaoConformidades;

public class ConclusaoNaoConformidadeModel : IConclusaoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public bool NovaReuniao { get; set; }
    public string Evidencia { get; set; }
    public DateTime? DataReuniao { get; set; }
    public DateTime DataVerificacao { get; set; }
    public Guid IdAuditor { get; set; }
    public bool Eficaz { get; set; }
    public int CicloDeTempo { get; set; }
    public Guid IdNovoRelatorio { get; set; }
    public Guid CompanyId { get; set; }

    public ConclusaoNaoConformidadeModel()
    {
    }

    public ConclusaoNaoConformidadeModel(IConclusaoNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        Evidencia = model.Evidencia;
        NovaReuniao = model.NovaReuniao;
        DataReuniao = model.DataReuniao;
        DataVerificacao = model.DataVerificacao;
        IdAuditor = model.IdAuditor;
        Eficaz = model.Eficaz;
        CicloDeTempo = model.CicloDeTempo;
        IdNovoRelatorio = model.IdNovoRelatorio;
        CompanyId = model.CompanyId;
    }
}