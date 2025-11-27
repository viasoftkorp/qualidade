using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ConclusaoNaoConformidades;

public interface IConclusaoNaoConformidadeModel
{
    Guid Id { get; }
    Guid IdNaoConformidade { get; }
    string Evidencia { get; }
    bool NovaReuniao { get; }
    DateTime? DataReuniao { get; }
    DateTime DataVerificacao { get; }
    Guid IdAuditor { get; }
    bool Eficaz { get; }
    int CicloDeTempo { get; }
    Guid IdNovoRelatorio { get; }
    Guid CompanyId { get; set; }

}