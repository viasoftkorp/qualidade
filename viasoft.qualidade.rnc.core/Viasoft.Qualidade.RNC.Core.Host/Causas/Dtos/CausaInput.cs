using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Causas.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;

public class CausaInput : CausaModel
{
    public CausaInput()
    {
    }

    public CausaInput(Causa causa)
    {
        Id = causa.Id;
        Descricao = causa.Descricao;
        Codigo = causa.Codigo;
        Detalhamento = causa.Detalhamento;
        IsAtivo = causa.IsAtivo;
    }
}