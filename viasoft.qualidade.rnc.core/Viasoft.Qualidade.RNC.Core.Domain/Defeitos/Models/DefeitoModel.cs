using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Models;

public class DefeitoModel
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public Guid? IdCausa { get; set; }
    public Guid? IdSolucao { get; set; }
    public bool IsAtivo { get; set; }
}