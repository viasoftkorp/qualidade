using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

public class CausaNaoConformidadeInput : CausaNaoConformidadeModel
{
    public CausaNaoConformidadeInput(CausaNaoConformidade causa)
    {
        Id = causa.Id;
        IdNaoConformidade = causa.IdNaoConformidade;
        IdDefeitoNaoConformidade = causa.IdDefeitoNaoConformidade;
        IdCausa = causa.IdCausa;
        Detalhamento = causa.Detalhamento;
    }

    public CausaNaoConformidadeInput()
    {
    }
}