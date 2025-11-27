using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

public class ServicoNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid? IdProduto { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    public int? Horas { get; set; }
    public int? Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    [IsArrayOfBytes]
    public string OperacaoEngenharia { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public string DescricaoRecurso { get; set; }
    public bool ControlarApontamento { get; set; }

    public ServicoNaoConformidadeViewOutput()
    {
    }
    public ServicoNaoConformidadeViewOutput(ServicoNaoConformidade servico, Produto produto, Recurso recurso)
    {
        Id = servico.Id;
        IdProduto = servico.IdProduto;
        IdNaoConformidade = servico.IdNaoConformidade;
        Quantidade = servico.Quantidade;
        Horas = servico.Horas;
        Minutos = servico.Minutos;
        IdRecurso = servico.IdRecurso;
        DescricaoRecurso = recurso.Descricao;
        OperacaoEngenharia = servico.OperacaoEngenharia;
        Detalhamento = servico.Detalhamento;
        ControlarApontamento = servico.ControlarApontamento;
        Codigo = produto.Codigo;
        Descricao = produto.Descricao;
        
    }
}