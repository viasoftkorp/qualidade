using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ReclamacoesNaoConformidades;

public interface IReclamacaoNaoConformidadeModel
{
    Guid Id { get;}
    Guid IdNaoConformidade { get;}
    int Procedentes { get; }
    int Improcedentes { get; }
    decimal QuantidadeLote { get; }
    decimal QuantidadeNaoConformidade { get; }
    int DisposicaoProdutosAprovados { get; }
    int DisposicaoProdutosConcessao { get; }
    int Retrabalho { get; }
    int Rejeitado { get; }
    bool RetrabalhoComOnus { get; }
    bool RetrabalhoSemOnus { get; }
    bool DevolucaoFornecedor { get; }
    bool Recodificar { get; }
    bool Sucata { get; }
    string Observacao { get; }
    Guid CompanyId { get; }
}