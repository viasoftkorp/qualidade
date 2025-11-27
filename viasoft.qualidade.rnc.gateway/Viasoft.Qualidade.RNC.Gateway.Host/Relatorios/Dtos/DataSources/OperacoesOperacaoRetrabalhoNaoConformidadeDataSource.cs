using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class OperacoesOperacaoRetrabalhoNaoConformidadeDataSource
{
    public List<RelatorioOperacoesOperacaoRetrabalhoNaoConformidade> OperacoesOperacaoRetrabalhoNaoConformidade { get; set; }
}

public class RelatorioOperacoesOperacaoRetrabalhoNaoConformidade
{
    public Guid Id { get; set; }
    public string NumeroOperacao { get; set; }
    public Guid IdRecurso { get; set; }
    public string DescricaoRecurso { get; set; }
    public string CodigoRecurso { get; set; }
    public Guid IdOperacaoRetrabalhoNaoConformdiade { get; set; }
    public string Status { get; set; }
}