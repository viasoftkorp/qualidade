using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Naturezas.Dtos;

public class NaturezaOutput
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    public bool IsAtivo { get; set; }
}