using System;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

public class OperacaoRetrabalhoNaoConformidade : EntityDto
{
    public Guid IdNaoConformidade { get; set; }
    public decimal Quantidade { get; set; }
    public string NumeroOperacaoARetrabalhar { get; set; }
}