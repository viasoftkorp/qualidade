using System;
using Viasoft.Core.DDD.Entities;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas.Dtos;

public class ProdutoEmpresaOutput: IEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }
    
    public ProdutoEmpresaOutput()
    {
    }
}