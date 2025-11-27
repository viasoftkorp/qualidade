using System;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades
    .Dtos;

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