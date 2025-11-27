using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Causas.Dtos;

public class CausaInput
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public int Codigo { get; set; }
}