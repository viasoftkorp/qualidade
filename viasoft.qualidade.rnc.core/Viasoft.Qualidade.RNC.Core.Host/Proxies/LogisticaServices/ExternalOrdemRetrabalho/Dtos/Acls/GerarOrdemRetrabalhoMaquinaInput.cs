using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;

public class GerarOrdemRetrabalhoMaquinaInput
{
    public string Operacao { get; set; }
    public Guid IdRecurso { get; set; }
    public string Detalhamento { get; set; }
    public int Horas { get; set; }
    
    public int Minutos { get; set; }

    public decimal TempoTotal => Horas + (Minutos / 60m);
}