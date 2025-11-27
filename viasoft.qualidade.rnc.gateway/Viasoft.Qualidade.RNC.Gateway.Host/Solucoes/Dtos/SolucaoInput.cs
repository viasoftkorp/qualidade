using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class SolucaoInput
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public int Codigo { get; set; }
    public bool Imediata { get; set; }
}