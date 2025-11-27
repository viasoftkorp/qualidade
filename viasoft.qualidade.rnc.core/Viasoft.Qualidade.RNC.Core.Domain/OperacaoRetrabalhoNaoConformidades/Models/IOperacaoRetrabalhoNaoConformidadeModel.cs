using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Models;

public interface IOperacaoRetrabalhoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public decimal Quantidade { get; set; }
    public string NumeroOperacaoARetrabalhar { get; set; }
}