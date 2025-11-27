using System;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

public class DefeitoNaoConformidadeOutput
{
    public Guid Id { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeito { get; set; }
    public decimal Quantidade { get; set; }

    public DefeitoNaoConformidadeOutput()
    {
    }

    public DefeitoNaoConformidadeOutput(DefeitoNaoConformidade defeito)
    {
        Id = defeito.Id;
        IdNaoConformidade = defeito.IdNaoConformidade;
        IdDefeito = defeito.IdDefeito;
        Detalhamento = defeito.Detalhamento;
        Quantidade = defeito.Quantidade;
    }
}