using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class ProdutoSolucaoInput
{
    public Guid Id { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid IdProduto { get; set; }
    public int Quantidade { get; set; }
    public string OperacaoEngenharia { get; set; }
}