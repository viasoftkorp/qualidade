using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Dtos;

public class LocalOutput
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public bool? IsBloquearMovimentacao { get; set; }
}