using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
public class MaquinaInput
{
    public Guid Id { get; set; }
    public string Detalhamento { get; set; }
    public int Horas { get; set; }
    public int Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    public List<MaterialInput> Materiais { get; set; } = new();
}
