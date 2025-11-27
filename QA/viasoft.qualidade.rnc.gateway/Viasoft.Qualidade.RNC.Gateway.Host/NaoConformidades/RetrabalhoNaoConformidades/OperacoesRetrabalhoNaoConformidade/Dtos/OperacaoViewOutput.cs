using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.
    OperacoesRetrabalhoNaoConformidade.Dtos;

public class OperacaoViewOutput
{
    public Guid Id { get; set; }
    public string NumeroOperacao { get; set; }
    public Guid IdRecurso { get; set; }
    public string DescricaoRecurso { get; set; }
    public string CodigoRecurso { get; set; }
    public Guid IdOperacaoRetrabalhoNaoConformdiade { get; set; }
    public StatusProducaoRetrabalho Status { get; set; }
}