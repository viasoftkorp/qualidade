using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendas.Providers;

public interface IEstoquePedidoVendaProvider
{
    public Task<PagedResultDto<EstoquePedidoVendaOutput>> GetList(GetListEstoquePedidoVendaInput input);
    public Task<EstoquePedidoVendaOutput> GetById(Guid id);
}

public class GetListEstoquePedidoVendaInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
}