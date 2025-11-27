using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;

public class GerarOrdemRetrabalhoMaterialInput
{
    public Guid IdProduto { get; set; }
    public string Operacao { get; set; }
    public decimal Quantidade { get; set; }
}