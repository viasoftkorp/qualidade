using System;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Models;

public interface IOrdemRetrabalhoNaoConformidadeModel
{
    public Guid IdNaoConformidade { get; set; }
    public int NumeroOdfRetrabalho { get; set; }
    public int Quantidade { get; set; }
    public string NumeroPedido { get; set; }
    public Guid IdEstoqueLocalOrigem { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }

}