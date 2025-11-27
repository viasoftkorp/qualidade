using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

public class MaquinaInput
{
    public Guid Id { get; set; }
    public string Detalhamento { get; set; }
    public int Horas { get; set; }
    public int Minutos { get; set; }
    public Guid IdRecurso { get; set; }
    public List<MaterialInput> Materiais { get; set; } = new();
}