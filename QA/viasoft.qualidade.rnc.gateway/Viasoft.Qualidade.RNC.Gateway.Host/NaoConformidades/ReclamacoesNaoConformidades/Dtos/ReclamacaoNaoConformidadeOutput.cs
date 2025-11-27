using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;

public class ReclamacaoNaoConformidadeOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public int Procedentes { get; set; }
    public int Improcedentes { get; set; }
    public decimal QuantidadeLote { get; set; }
    public decimal QuantidadeNaoConformidade { get; set; }
    public int DisposicaoProdutosAprovados { get; set; }
    public int DisposicaoProdutosConcessao { get; set; }
    public int Rejeitado { get; set; }
    public int Retrabalho { get; set; }
    public bool RetrabalhoComOnus { get; set; }
    public bool RetrabalhoSemOnus { get; set; }
    public bool DevolucaoFornecedor { get; set; }
    public bool Recodificar { get; set; }
    public bool Sucata { get; set; }
    public string Observacao { get; set; }
}