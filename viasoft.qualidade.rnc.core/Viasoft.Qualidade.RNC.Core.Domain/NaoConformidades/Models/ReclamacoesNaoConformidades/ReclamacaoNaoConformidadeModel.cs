using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ReclamacoesNaoConformidades;

public class ReclamacaoNaoConformidadeModel : IReclamacaoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public int Procedentes { get; set; }
    public int Improcedentes { get; set; }
    public decimal QuantidadeLote { get; set; }
    public decimal QuantidadeNaoConformidade { get; set; }
    public int DisposicaoProdutosAprovados { get; set; }
    public int DisposicaoProdutosConcessao { get; set; }
    public int Retrabalho { get; set; }
    public int Rejeitado { get; set; }
    public bool RetrabalhoComOnus { get; set; }
    public bool RetrabalhoSemOnus { get; set; }
    public bool DevolucaoFornecedor { get; set; }
    public bool Recodificar { get; set; }
    public bool Sucata { get; set; }
    public string Observacao { get; set; }
    public Guid CompanyId { get; set; }

    public ReclamacaoNaoConformidadeModel()
    {
    }

    public ReclamacaoNaoConformidadeModel(IReclamacaoNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        Procedentes = model.Procedentes;
        Improcedentes = model.Improcedentes;
        QuantidadeLote = model.QuantidadeLote;
        QuantidadeNaoConformidade = model.QuantidadeNaoConformidade;
        DisposicaoProdutosAprovados = model.DisposicaoProdutosAprovados;
        DisposicaoProdutosConcessao = model.DisposicaoProdutosConcessao;
        Retrabalho = model.Retrabalho;
        Rejeitado = model.Rejeitado;
        RetrabalhoComOnus = model.RetrabalhoComOnus;
        RetrabalhoSemOnus = model.RetrabalhoSemOnus;
        DevolucaoFornecedor = model.DevolucaoFornecedor;
        Recodificar = model.Recodificar;
        Sucata = model.Sucata;
        Observacao = model.Observacao;
        CompanyId = model.CompanyId;
    }
}