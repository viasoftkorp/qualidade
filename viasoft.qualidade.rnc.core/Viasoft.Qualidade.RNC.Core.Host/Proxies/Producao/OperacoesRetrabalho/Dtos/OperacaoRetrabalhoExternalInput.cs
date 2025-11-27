using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;

public class OperacaoRetrabalhoExternalInput
{
    public string Maquina { get; set; }
    public int Hora { get; set; }
    public int Minuto { get; set; }
    public int Segundo { get; set; }
    public string DescricaoOperacao { get; set; }
    public List<MaterialRetrabalhoExternalInput> Materiais { get; set; }
}