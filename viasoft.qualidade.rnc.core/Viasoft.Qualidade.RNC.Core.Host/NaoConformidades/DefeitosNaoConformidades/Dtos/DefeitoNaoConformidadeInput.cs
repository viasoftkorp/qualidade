using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.DefeitosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

public class DefeitoNaoConformidadeInput : DefeitoNaoConformidadeModel
{
    public DefeitoNaoConformidadeInput()
    {
    }

    public DefeitoNaoConformidadeInput(DefeitoNaoConformidade defeito)
    {
        Detalhamento = defeito.Detalhamento;
        Id = defeito.Id;
        Quantidade = defeito.Quantidade;
        IdDefeito = defeito.IdDefeito;
        IdNaoConformidade = defeito.IdNaoConformidade;

    }
}