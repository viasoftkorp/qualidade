using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.SolucoesNaoConformidades;

public interface ISolucaoNaoConformidadeModel
{
    Guid Id { get; }
    Guid IdNaoConformidade { get; }
    Guid IdDefeitoNaoConformidade { get; }
    public Guid IdSolucao { get; }
    public string Detalhamento { get; }
    bool SolucaoImediata { get; }
    DateTime? DataAnalise { get; }
    DateTime? DataPrevistaImplantacao { get; }
    Guid? IdResponsavel { get; }
    decimal CustoEstimado { get; }
    DateTime? NovaData { get; }
    DateTime? DataVerificacao { get; }
    Guid? IdAuditor { get; }
    Guid CompanyId { get; }
}