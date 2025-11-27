using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;

public class ConclusaoNaoConformidadeOutput
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
}