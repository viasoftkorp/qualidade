using System;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes.Models;

public interface IOperacaoModel
{
    public string NumeroOperacao { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdOperacaoRetrabalhoNaoConformdiade {get; set; }
    public StatusProducaoRetrabalho Status { get; set; }
}