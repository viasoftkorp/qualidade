using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

public class DefeitoNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeito { get; set; }
    public decimal Quantidade { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    
    public DefeitoNaoConformidadeViewOutput()
    {
    }

    public DefeitoNaoConformidadeViewOutput(DefeitoNaoConformidade defeitoNaoConformidade, Defeito defeito)
    {
        Id = defeitoNaoConformidade.Id;
        Codigo = defeito.Codigo;
        Descricao = defeito.Descricao;
        IdNaoConformidade = defeitoNaoConformidade.IdNaoConformidade;
        IdDefeito = defeito.Id;
        Quantidade = defeitoNaoConformidade.Quantidade;
        Detalhamento = defeitoNaoConformidade.Detalhamento;
    }
    
}