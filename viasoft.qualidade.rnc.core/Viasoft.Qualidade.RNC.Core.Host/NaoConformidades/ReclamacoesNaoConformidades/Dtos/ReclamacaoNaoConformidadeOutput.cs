using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ReclamacoesNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;

public class ReclamacaoNaoConformidadeOutput : ReclamacaoNaoConformidadeModel
{
    public ReclamacaoNaoConformidadeOutput(ReclamacaoNaoConformidade reclamacao)
    {
        Id = reclamacao.Id;
        IdNaoConformidade = reclamacao.IdNaoConformidade;
        Procedentes = reclamacao.Procedentes;
        Improcedentes = reclamacao.Improcedentes;
        QuantidadeLote = reclamacao.QuantidadeLote;
        QuantidadeNaoConformidade = reclamacao.QuantidadeNaoConformidade;
        DisposicaoProdutosAprovados = reclamacao.DisposicaoProdutosAprovados;
        DisposicaoProdutosConcessao = reclamacao.DisposicaoProdutosConcessao;
        Retrabalho = reclamacao.Retrabalho;
        Rejeitado = reclamacao.Rejeitado;
        RetrabalhoComOnus = reclamacao.RetrabalhoComOnus;
        RetrabalhoSemOnus = reclamacao.RetrabalhoSemOnus;
        DevolucaoFornecedor = reclamacao.DevolucaoFornecedor;
        Recodificar = reclamacao.Recodificar;
        Sucata = reclamacao.Sucata;
        Observacao = reclamacao.Observacao;
    }
}