namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;

public class ExternalGerarOrdemRetrabalhoOutput
{  
    public int OdfGerada { get; set; }
    public string Message  { get; set; }
    public bool Success { get; set; } = true;
}