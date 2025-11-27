using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades.Models;

public interface IServicoNaoConformidadeModel
{
    Guid Id { get; }
    Guid? IdProduto { get; }
    Guid IdNaoConformidade { get; }
    decimal Quantidade { get; }
    int Horas { get; }
    int Minutos { get; }
    Guid IdRecurso { get; }
    string OperacaoEngenharia { get; }
    public string Detalhamento { get; }
    Guid CompanyId { get; set; }

}