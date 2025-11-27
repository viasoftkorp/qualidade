using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

public class SolucaoNaoConformidadeInput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdSolucao { get; set; }
    public string Detalhamento { get; set; }
    public bool SolucaoImediata { get; set; }
    public bool AcaoImediata { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public decimal CustoEstimado { get; set; }
    public DateTime? NovaData { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public Guid? IdAuditor { get; set; }
}