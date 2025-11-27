using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Providers;

public interface IEstoquePedidoVendaEstoqueLocalProvider
{
    public Task<PagedResultDto<EstoquePedidoVendaEstoqueLocalOutput>> GetList(ListEstoquePedidoVendaEstoqueLocalInput input);
    public Task<EstoquePedidoVendaEstoqueLocalOutput> GetById(Guid id);
}
public class ListEstoquePedidoVendaEstoqueLocalInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
}