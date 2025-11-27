using System;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

public class OrdemRetrabalhoNaoConformidadeOutput
{
    public Guid IdNaoConformidade { get; set; }
    public int NumeroOdfRetrabalho { get; set; }
    public decimal Quantidade { get; set; }
    public Guid IdLocalOrigem { get; set; }
    public Guid? IdEstoqueLocalDestino { get; set; }
    public Guid IdLocalDestino { get; set; }
    public string CodigoArmazem { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }
    public string Message  { get; set; }
    public bool Success { get; set; } = true;

    public OrdemRetrabalhoNaoConformidadeOutput()
    {
        
    }

    public OrdemRetrabalhoNaoConformidadeOutput(OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade)
    {
        IdNaoConformidade = ordemRetrabalhoNaoConformidade.IdNaoConformidade;
        NumeroOdfRetrabalho = ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho;
        Quantidade = ordemRetrabalhoNaoConformidade.Quantidade;
        IdLocalOrigem = ordemRetrabalhoNaoConformidade.IdLocalOrigem;
        IdEstoqueLocalDestino = ordemRetrabalhoNaoConformidade.IdEstoqueLocalDestino;
        IdLocalDestino = ordemRetrabalhoNaoConformidade.IdLocalDestino;
        CodigoArmazem = ordemRetrabalhoNaoConformidade.CodigoArmazem;
        DataFabricacao = ordemRetrabalhoNaoConformidade.DataFabricacao;
        DataValidade = ordemRetrabalhoNaoConformidade.DataValidade;
        Status = ordemRetrabalhoNaoConformidade.Status;
    }
}