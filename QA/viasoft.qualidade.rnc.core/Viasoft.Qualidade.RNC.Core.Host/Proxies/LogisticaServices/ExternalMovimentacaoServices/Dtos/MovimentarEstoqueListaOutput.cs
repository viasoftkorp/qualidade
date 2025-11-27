namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

public class MovimentarEstoqueListaOutput
{
    public bool Success { get; set; } = true;
    public string Message { get; set; }
    
    public ExternalMovimentarEstoqueItemResultado DtoRetorno { get; set; }
}