using System;
using System.Collections.Generic;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas.Dtos;

public class GetProdutoEmpresaListInput : PagedFilteredAndSortedRequestInput
{
    public List<Guid> ProductsIds { get; set; } = new();
    public Guid? CompanyId { get; set; }

    public GetProdutoEmpresaListInput() { }
}
