using System;
using Viasoft.Core.DDD.Entities;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Models;

public class OrdemRetrabalhoNaoConformidadeModel : IOrdemRetrabalhoNaoConformidadeModel, IEntity
{
    public Guid IdNaoConformidade { get; set; }
    public int NumeroOdfRetrabalho { get; set; }
    public int Quantidade { get; set; }
    public string NumeroPedido { get; set; }
    public Guid Id { get; set; }
    public Guid IdEstoqueLocalOrigem { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }
    public string MovimentacaoEstoqueMensagemRetorno { get; set; }

}