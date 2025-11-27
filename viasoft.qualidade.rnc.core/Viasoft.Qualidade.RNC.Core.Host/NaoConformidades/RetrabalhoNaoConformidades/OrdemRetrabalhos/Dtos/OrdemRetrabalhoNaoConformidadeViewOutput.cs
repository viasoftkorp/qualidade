using System;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

public class OrdemRetrabalhoNaoConformidadeViewOutput
{
    public Guid IdNaoConformidade { get; set; }
    public int NumeroOdfRetrabalho { get; set; }
    public decimal Quantidade { get; set; }
    public Guid IdLocalOrigem { get; set; }
    public string DescricaoLocalOrigem { get; set; }
    public int CodigoLocalOrigem { get; set; }
    public Guid? IdEstoqueLocalDestino { get; set; }
    public Guid IdLocalDestino { get; set; }
    public string DescricaoLocalDestino { get; set; }
    public int CodigoLocalDestino { get; set; }
    public string CodigoArmazem { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }
}