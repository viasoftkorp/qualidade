using System;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.CentroCustos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Dtos;

public class CentroCustoOutput
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public bool IsSintetico { get; set; }

    public CentroCustoOutput()
    {
        
    }
    public CentroCustoOutput(CentroCusto centroCusto)
    {
        Id = centroCusto.Id;
        Codigo = centroCusto.Codigo;
        Descricao = centroCusto.Descricao;
        IsSintetico = centroCusto.IsSintetico;
    }
}