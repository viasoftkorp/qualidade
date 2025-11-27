using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Dtos;

public class AcaoPreventivaOutput
{
    public Guid Id { get; set; }

    public int Codigo { get; set; }

    public string Descricao { get; set; }

    public string Detalhamento { get; set; }

    public Guid? IdResponsavel { get; set; }
    public bool IsAtivo { get; set; }
}