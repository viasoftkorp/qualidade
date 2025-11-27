using System;
using System.Collections.Generic;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class GetProdutosListInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdCategoria { get; set; }
    public string CodigoCategoria { get; set; }
}