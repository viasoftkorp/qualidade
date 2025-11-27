using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.Naturezas.Model;

public class NaturezaModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    public bool IsAtivo { get; set; }
}