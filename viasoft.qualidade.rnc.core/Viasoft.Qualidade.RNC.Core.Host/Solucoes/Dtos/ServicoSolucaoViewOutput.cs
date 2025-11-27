using System;
using Viasoft.Core.DynamicLinqQueryBuilder;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class ServicoSolucaoViewOutput
{
    public Guid Id { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid? IdProduto { get; set; }
    public string Codigo { get; set; }
    public int Quantidade { get; set; }
    public string Descricao { get; set; }
    public int? Horas { get; set; }
    public int? Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    [IsArrayOfBytes]
    public string OperacaoEngenharia { get; set; }
    public string Recurso { get; set; }
    public string Produto { get; set; }
}