using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.DefeitosNaoConformidades;

public class DefeitoNaoConformidadeModel : IDefeitoNaoConformidadeModel
{
    public Guid Id { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeito { get; set; }
    public decimal Quantidade { get; set; }
    public Guid CompanyId { get; set; }

    public DefeitoNaoConformidadeModel()
    {
    }

    public DefeitoNaoConformidadeModel(IDefeitoNaoConformidadeModel model)
    {
        Id = model.Id;
        IdNaoConformidade = model.IdNaoConformidade;
        IdDefeito = model.IdDefeito;
        Detalhamento = model.Detalhamento;
        Quantidade = model.Quantidade;
        CompanyId = model.CompanyId;
    }
}