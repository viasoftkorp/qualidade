using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.AcoesPreventivasNaoConformidades;

public interface IAcaoPreventivaNaoConformidadeModel
{
    Guid Id { get; }
    Guid IdDefeitoNaoConformidade { get; }
    Guid IdAcaoPreventiva { get; }
    Guid IdNaoConformidade { get;}
    string Acao { get;}
    string Detalhamento { get;}
    Guid? IdResponsavel { get;}
    DateTime? DataAnalise { get;}
    DateTime? DataPrevistaImplantacao { get;}
    Guid? IdAuditor { get;}
    bool Implementada { get;}
    DateTime? DataVerificacao { get;}
    DateTime? NovaData { get;}
    Guid CompanyId { get; }
}