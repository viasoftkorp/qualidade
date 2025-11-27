using System;
using System.Collections.Generic;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;

public class GerarOrdemRetrabalhoInput
{
    public string NumeroPedido { get; set; }
    public int NumeroOdfOrigem { get; set; }
    public string NumeroLote { get; set; }
    public Guid IdLocalDestino { get; set; }
    public decimal Quantidade { get; set; }
    public Guid IdProduto { get; set; }
    public Guid? IdPessoa { get; set; }
    public List<GerarOrdemRetrabalhoMaterialInput> MateriaisInput { get; set; }
    public List<GerarOrdemRetrabalhoMaquinaInput> MaquinasInput { get; set; }
}