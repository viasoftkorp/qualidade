using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFinanceiros.CentroCustos.Dtos;

public class CentroCustoOutput
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public bool IsSintetico { get; set; }
}