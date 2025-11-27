using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class ServicoSolucaoOutput
{
    public Guid Id { get; set; }
    public Guid IdSolucao { get; set; }
    public Guid? IdProduto { get; set; }
    public int Quantidade { get; set; }
    public int? Horas { get; set; }
    public int? Minutos { get; set; }
    public Guid? IdRecurso { get; set; }
    public string OperacaoEngenharia { get; set; }
}