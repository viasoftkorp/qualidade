using System;

namespace Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais.Dtos;

public class EstoqueLocalOutput
{
    public Guid Id { get; set; }
    public decimal Quantidade { get; set; }
    public int CodigoLocal { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public string CodigoArmazem { get; set; }
    public Guid IdLocal { get; set; }
}