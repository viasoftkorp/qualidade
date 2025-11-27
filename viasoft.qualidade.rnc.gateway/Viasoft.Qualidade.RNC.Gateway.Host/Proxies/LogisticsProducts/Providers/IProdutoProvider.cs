using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Utils;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LogisticsProducts.Providers;

public interface IProdutoProvider
{
    Task<PagedResultDto<ProdutoOutput>> GetProdutosList(PagedFilteredAndSortedRequestInput input);
    Task<PagelessResultDto<ProdutoOutput>> GetPagelessProdutosList(PagedFilteredAndSortedRequestInput input);
    Task<ProdutoOutput> GetProduto(Guid id);
    Task<ProdutoOutput> GetByCode(string code);
}