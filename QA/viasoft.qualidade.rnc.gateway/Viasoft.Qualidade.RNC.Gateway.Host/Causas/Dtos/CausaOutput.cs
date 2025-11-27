using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Causas.Dtos;

public class CausaOutput
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public int Codigo { get; set; }
    public bool IsAtivo { get; set; }

}