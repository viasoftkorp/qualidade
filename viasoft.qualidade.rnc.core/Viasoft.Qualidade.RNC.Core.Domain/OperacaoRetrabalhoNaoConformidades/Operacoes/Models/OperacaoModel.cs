using System;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes.Models;

public class OperacaoModel : IOperacaoModel
{
    public string NumeroOperacao { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdOperacaoRetrabalhoNaoConformdiade { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }

    public OperacaoModel()
    {
        
    }

    public OperacaoModel(Operacao operacao)
    {
        NumeroOperacao = operacao.NumeroOperacao;
        IdRecurso = operacao.IdRecurso;
        IdOperacaoRetrabalhoNaoConformdiade = operacao.IdOperacaoRetrabalhoNaoConformdiade;
        Status = operacao.Status;
    }
}