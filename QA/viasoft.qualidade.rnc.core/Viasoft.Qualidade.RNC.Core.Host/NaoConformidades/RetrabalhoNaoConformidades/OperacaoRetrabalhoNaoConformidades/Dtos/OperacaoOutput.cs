using System;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

public class OperacaoOutput : OperacaoModel
{
    public Guid Id { get; set; }
    public OperacaoOutput(Operacao operacao):base(operacao)
    {
        Id = operacao.Id;
    }
}