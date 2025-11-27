using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Dtos;

public class DefeitoInput
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public int Codigo { get; set; }
    public Guid? IdCausa { get; set; }
    public Guid? IdSolucao { get; set; }
}