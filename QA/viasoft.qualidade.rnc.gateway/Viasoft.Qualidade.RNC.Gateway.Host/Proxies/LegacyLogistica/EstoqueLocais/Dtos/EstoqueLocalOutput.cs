using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Dtos;

public class EstoqueLocalOutput
{
    public Guid Id { get; set; }
    public int LegacyId { get; set; }
    public string CodigoProduto { get; set; }
    public Guid IdProduto { get; set; }
    
    public string Lote { get; set; }
    
    public int LegacyIdEmpresa { get; set; }
    public Guid IdEmpresa { get; set; }
    
    public decimal Quantidade { get; set; }
    public int CodigoLocal { get; set; }
    public Guid IdLocal { get; set; }
    public int? NumeroAlocacao { get; set; }
    public DateTime? DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public string CodigoArmazem { get; set; }
    public bool IsLocalBloquearMovimentacao { get; set; }
}