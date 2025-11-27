using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

public class CausaNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public Guid IdCausa { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }

    public CausaNaoConformidadeViewOutput()
    {
    }

    public CausaNaoConformidadeViewOutput(CausaNaoConformidade causaNaoConformidade, Causa causa)
    {
        Id = causaNaoConformidade.Id;
        IdNaoConformidade = causaNaoConformidade.IdNaoConformidade;
        IdDefeitoNaoConformidade = causaNaoConformidade.IdDefeitoNaoConformidade;
        Detalhamento = causaNaoConformidade.Detalhamento;
        IdCausa = causaNaoConformidade.IdCausa;
        Codigo = causa.Codigo;
        Descricao = causa.Descricao;
    }

}