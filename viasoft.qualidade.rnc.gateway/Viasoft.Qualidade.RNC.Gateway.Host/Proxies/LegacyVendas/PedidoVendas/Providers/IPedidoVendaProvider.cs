using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyVendas.PedidoVendas.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyVendas.PedidoVendas.Providers;

public interface IPedidoVendaProvider
{
    public Task<PagedResultDto<PedidoVendaOutput>> GetList(GetListPedidoVendaInput input);
    public Task<PedidoVendaOutput> GetById(Guid id);
}
public class GetListPedidoVendaInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
}