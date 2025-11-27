using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OperacoesOrdemProducao.Dtos;

public class OperacaoOrdemProducaoDto
{
    public int IdOperacao { get; set; }
    public string Operacao { get; set; }
    public string Sequencia { get; set; }
    public int Ranking { get; set; }
    public int IdEmpresa { get; set; }
    public string Maquina { get; set; }
    public string DescricaoMaquina { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? DataInicio { get; set; }
    public decimal QuantidadePecasPorHora { get; set; }
    public bool UtilizaCracha { get; set; }
    public string Posicao { get; set; }
}

public class GetOperacaoOrdemProducaoDto
{
    public List<OperacaoOrdemProducaoDto> Operacoes { get; set; }
    public int TotalCount { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
}