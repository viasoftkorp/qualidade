using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

public class AcaoPreventivaNaoConformidadeInput
{
    public Guid Id { get; set; }
    public Guid IdAcaoPreventiva { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public string Acao { get; set; }
    public string Detalhamento { get; set; }
    public Guid? IdResponsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public bool Implementada { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }
}