using System;
using Viasoft.Core.DynamicLinqQueryBuilder;

namespace Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Models;

public class SolucaoModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public bool Imediata { get; set; }
    public bool IsAtivo { get; set; }

}