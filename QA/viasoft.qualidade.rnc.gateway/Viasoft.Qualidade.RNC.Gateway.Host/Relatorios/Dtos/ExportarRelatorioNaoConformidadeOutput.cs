namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;

public class ExportarRelatorioNaoConformidadeOutput
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public byte[] FileBytes { get; set; }
}