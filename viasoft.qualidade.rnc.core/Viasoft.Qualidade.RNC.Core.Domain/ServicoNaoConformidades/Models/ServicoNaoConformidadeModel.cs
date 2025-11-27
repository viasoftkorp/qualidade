using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades.Models;

public class ServicoNaoConformidadeModel : IServicoNaoConformidadeModel
{
    public Guid IdNaoConformidade { get; set;}
    public Guid Id { get; set; }
    public Guid? IdProduto { get; set;}
    public decimal Quantidade { get;set; }
    public int Horas { get;set; }
    public int Minutos { get;set; }
    public Guid IdRecurso { get;set; }
    public string OperacaoEngenharia { get; set; }
    public string Detalhamento { get; set; }
    public Guid CompanyId { get; set; }

    public ServicoNaoConformidadeModel()
    {
    }

    public ServicoNaoConformidadeModel(IServicoNaoConformidadeModel model)
    {
        Id = model.Id;
        IdProduto = model.IdProduto;
        IdNaoConformidade = model.IdNaoConformidade;
        Quantidade = model.Quantidade;
        Detalhamento = model.Detalhamento;
        Horas = model.Horas;
        Minutos = model.Minutos;
        IdRecurso = model.IdRecurso;
        OperacaoEngenharia = model.OperacaoEngenharia;
        CompanyId = model.CompanyId;
    }
}
