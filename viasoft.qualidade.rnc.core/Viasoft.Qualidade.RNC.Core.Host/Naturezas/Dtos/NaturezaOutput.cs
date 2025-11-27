using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas.Model;

namespace Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;

public class NaturezaOutput : NaturezaModel
{
    
    public NaturezaOutput(Natureza natureza)
    {
        Id = natureza.Id;
        Descricao = natureza.Descricao;
        Codigo = natureza.Codigo;
        IsAtivo = natureza.IsAtivo;
    }

    public NaturezaOutput()
    {
    }
}