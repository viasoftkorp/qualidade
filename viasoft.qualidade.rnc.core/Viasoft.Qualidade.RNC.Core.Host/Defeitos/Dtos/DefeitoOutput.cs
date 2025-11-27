using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;

public class DefeitoOutput : DefeitoModel
{
    public DefeitoOutput(Defeito defeito)
    {
        Id = defeito.Id;
        Codigo = defeito.Codigo;
        Descricao = defeito.Descricao;
        Detalhamento = defeito.Detalhamento;
        IdCausa = defeito?.IdCausa;
        IdSolucao = defeito?.IdSolucao;
        IsAtivo = defeito.IsAtivo;
    }
}

