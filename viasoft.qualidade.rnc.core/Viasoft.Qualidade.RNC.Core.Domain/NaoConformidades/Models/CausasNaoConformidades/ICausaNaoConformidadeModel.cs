using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;

public interface ICausaNaoConformidadeModel
{
    Guid Id { get; }
    Guid IdNaoConformidade { get; }
    Guid IdDefeitoNaoConformidade { get; }
    string Detalhamento { get; }
    Guid IdCausa { get; }
    Guid CompanyId { get; }
    List<Guid> IdsCentrosCustos { get; }
}
