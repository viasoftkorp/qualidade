using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;

public class AcaoPreventivaViewOutput : AcaoPreventivaModel
{
    public string NomeResponsavel { get; set; }

    public AcaoPreventivaViewOutput()
    {
        
    }
    public AcaoPreventivaViewOutput(AcaoPreventiva acaoPreventiva)
    {
        Id = acaoPreventiva.Id;
        Codigo = acaoPreventiva.Codigo;
        Descricao = acaoPreventiva.Descricao;
        Detalhamento = acaoPreventiva.Detalhamento;
        IdResponsavel = acaoPreventiva.IdResponsavel;
        IsAtivo = acaoPreventiva.IsAtivo;
    }
}