using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Providers;

public interface IItemNotaFiscalEntradaRateioLoteProvider
{
    public Task<PagedResultDto<ItemNotaFiscalEntradaRateioLoteOutput>> GetList(GetListItemNotaFiscalRateioLoteInput input);
    public Task<ItemNotaFiscalEntradaRateioLoteOutput> GetById(Guid id);
}

public class GetListItemNotaFiscalRateioLoteInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
}