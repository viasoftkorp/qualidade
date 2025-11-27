using System;
using Viasoft.Data.Attributes;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

public class ProdutoSolucaoModel
{
    public Guid Id { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid IdProduto { get; set; }
    [StrictRequired]
    public int Quantidade { get; set; }
    public string OperacaoEngenharia { get; set; }

}