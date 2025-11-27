using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class OrdemRetrabalhoNaoConformidadeDataSource
{
    public List<RelatorioOrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidade { get; set; }
}

public class RelatorioOrdemRetrabalhoNaoConformidade
{
    public Guid IdNaoConformidade { get; set; }
    public int NumeroOdfRetrabalho { get; set; }
    public decimal Quantidade { get; set; }
    public Guid IdLocalOrigem { get; set; }
    public string DescricaoLocalOrigem { get; set; }
    public int CodigoLocalOrigem { get; set; }
    public Guid? IdEstoqueLocalDestino { get; set; }
    public Guid IdLocalDestino { get; set; }
    public string DescricaoLocalDestino { get; set; }
    public int CodigoLocalDestino { get; set; }
    public string CodigoArmazem { get; set; }
    public string DataFabricacao { get; set; }
    public string DataValidade { get; set; }
    public string Status { get; set; }
}