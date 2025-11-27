using System;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;

public class LocalOutput
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public bool? IsBloquearMovimentacao { get; set; }

    public LocalOutput()
    {
        
    }
    public LocalOutput(Local local)
    {
        Id = local.Id;
        Codigo = local.Codigo;
        Descricao = local.Descricao;
        IsBloquearMovimentacao = local.IsBloquearMovimentacao;
    }
    
}