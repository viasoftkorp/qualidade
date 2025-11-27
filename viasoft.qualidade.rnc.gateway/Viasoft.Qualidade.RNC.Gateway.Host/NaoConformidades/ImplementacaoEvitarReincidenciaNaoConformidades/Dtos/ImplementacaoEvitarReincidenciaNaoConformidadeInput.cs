using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

public class ImplementacaoEvitarReincidenciaNaoConformidadeInput
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
}