using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class ProdutoSolucaoViewOutput
{
    public Guid Id { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid IdProduto { get; set; }
    public int Quantidade { get; set; }
    public string Descricao { get; set; }
    public string UnidadeMedida { get; set; }
    public string Codigo { get; set; }
    public string Produto { get; set; }
    public string OperacaoEngenharia { get; set; }
}