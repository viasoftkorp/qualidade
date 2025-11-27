using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;

public interface IItemNotaFiscalEntradaProvider
{
    public Task<PagedResultDto<ItemNotaFiscalEntradaOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    public Task<ItemNotaFiscalEntradaOutput> GetById(Guid id);
}