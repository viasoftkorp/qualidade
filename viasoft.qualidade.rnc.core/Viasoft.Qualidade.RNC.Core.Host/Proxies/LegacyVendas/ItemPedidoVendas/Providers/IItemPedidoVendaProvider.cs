using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyVendas.ItemPedidoVendas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyVendas.ItemPedidoVendas.Providers;

public interface IItemPedidoVendaProvider
{
    public Task<PagedResultDto<ItemPedidoVendaOutput>> GetList(GetListItemPedidoVendaInput input);
}

public class GetListItemPedidoVendaInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
}