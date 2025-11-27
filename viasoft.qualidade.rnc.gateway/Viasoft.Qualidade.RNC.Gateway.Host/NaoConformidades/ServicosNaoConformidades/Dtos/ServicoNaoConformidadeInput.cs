using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

public class ServicoNaoConformidadeInput
{
    public Guid Id { get; set; }
    public Guid? IdProduto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    public int? Horas { get; set; }
    public int? Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    public string OperacaoEngenharia { get; set; }
    public string Detalhamento { get; set; }
    public bool ControlarApontamento { get; set; }
}