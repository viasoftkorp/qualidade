using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Providers;

public interface ICategoriaProdutoProvider
{
    public Task<PagedResultDto<CategoriaProdutoOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    public Task<List<CategoriaProdutoOutput>> GetAllCategoriasPaginando(List<Guid> idsCategorias);
}