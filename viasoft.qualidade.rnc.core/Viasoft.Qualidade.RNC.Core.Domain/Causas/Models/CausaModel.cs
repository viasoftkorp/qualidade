using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.Causas.Models;

public class CausaModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    [IsArrayOfBytes] public string Detalhamento { get; set; }
    public bool IsAtivo { get; set; }
}