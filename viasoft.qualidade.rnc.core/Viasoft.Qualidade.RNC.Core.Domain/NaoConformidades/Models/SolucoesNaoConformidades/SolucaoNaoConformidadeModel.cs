using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.SolucoesNaoConformidades;

public class SolucaoNaoConformidadeModel : ISolucaoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public bool SolucaoImediata { get; set; }
    public Guid IdSolucao { get; set; }
    public string Detalhamento { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdResponsavel { get; set; }
    public decimal CustoEstimado { get; set; }
    public DateTime? NovaData { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public Guid CompanyId { get; set; }

    public SolucaoNaoConformidadeModel()
    {
    }

    public SolucaoNaoConformidadeModel(ISolucaoNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        IdDefeitoNaoConformidade = model.IdDefeitoNaoConformidade;
        SolucaoImediata = model.SolucaoImediata;
        DataAnalise = model.DataAnalise;
        DataPrevistaImplantacao = model.DataPrevistaImplantacao;
        IdResponsavel = model.IdResponsavel;
        CustoEstimado = model.CustoEstimado;
        NovaData = model.NovaData;
        DataVerificacao = model.DataVerificacao;
        IdAuditor = model.IdAuditor;
        Detalhamento = model.Detalhamento;
        IdSolucao = model.IdSolucao;
        CompanyId = model.CompanyId;
    }
}