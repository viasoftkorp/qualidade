using System;
using Viasoft.Core.DynamicLinqQueryBuilder;

namespace Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas.Models;

public class AcaoPreventivaModel
{
    public Guid Id { get; set; }

    public string Descricao { get; set; }

    public int Codigo { get; set; }
    [IsArrayOfBytes]

    public string Detalhamento { get; set; }

    public Guid? IdResponsavel { get; set; }
    public bool IsAtivo { get; set; }
}