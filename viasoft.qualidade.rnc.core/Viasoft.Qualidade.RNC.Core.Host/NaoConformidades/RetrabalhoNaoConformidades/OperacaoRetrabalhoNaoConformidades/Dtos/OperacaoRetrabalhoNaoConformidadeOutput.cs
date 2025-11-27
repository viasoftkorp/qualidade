using System;
using System.Collections.Generic;
using System.Linq;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

public class OperacaoRetrabalhoNaoConformidadeOutput : IOperacaoRetrabalhoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    public string NumeroOperacaoARetrabalhar { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public List<OperacaoOutput> Operacoes { get; set; }
    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidationResult { get; set; }

    public OperacaoRetrabalhoNaoConformidadeOutput()
    {
        
    }
    public OperacaoRetrabalhoNaoConformidadeOutput(OperacaoRetrabalhoNaoConformidade operacaoRetrabalhoNaoConformidade)
    {
        var operacoes = operacaoRetrabalhoNaoConformidade.Operacoes
            .Select(e => new OperacaoOutput(e))
            .ToList();
        
        Id = operacaoRetrabalhoNaoConformidade.Id;
        IdNaoConformidade = operacaoRetrabalhoNaoConformidade.IdNaoConformidade;
        Quantidade = operacaoRetrabalhoNaoConformidade.Quantidade;
        NumeroOperacaoARetrabalhar = operacaoRetrabalhoNaoConformidade.NumeroOperacaoARetrabalhar;
        Operacoes = operacoes;
    }
}