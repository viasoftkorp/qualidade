using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.DefeitosNaoConformidades;

public interface IDefeitoNaoConformidadeModel
{
    Guid Id { get; }
    string Detalhamento { get; }
    Guid IdNaoConformidade { get; }
    Guid IdDefeito { get; } 
    decimal Quantidade { get; }
    Guid CompanyId { get; }
}