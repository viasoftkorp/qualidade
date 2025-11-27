using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;

public interface IItemNotaFiscalEntradaProvider
{
    public Task<PagedResultDto<ItemNotaFiscalEntradaOutput>> GetList(GetListItemNotaFiscalInput input);
    public Task<ItemNotaFiscalEntradaOutput> GetById(Guid id);
}

public class GetListItemNotaFiscalInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdNotaFiscal { get; set; }
    public Guid? IdEmpresa { get; set; }
}