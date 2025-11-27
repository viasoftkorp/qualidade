using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Dtos;

public class DefeitoViewOutput
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public string Detalhamento { get; set; }
    public Guid? IdCausa { get; set; }
    public Guid? IdSolucao { get; set; }
    public string DescricaoCausa { get; set; }
    public int? CodigoCausa { get; set; }
    public string DescricaoSolucao { get; set; }
    public int? CodigoSolucao { get; set; }
    public string Causa { get; set; }
    public string Solucao { get; set; }
    public bool IsAtivo { get; set; }
}