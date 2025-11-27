using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

public class OrdemRetrabalhoInput
{
    public decimal Quantidade { get; set; }
    public Guid IdLocalDestino { get; set; }
    public Guid? IdEstoqueLocalOrigem { get; set; }
}