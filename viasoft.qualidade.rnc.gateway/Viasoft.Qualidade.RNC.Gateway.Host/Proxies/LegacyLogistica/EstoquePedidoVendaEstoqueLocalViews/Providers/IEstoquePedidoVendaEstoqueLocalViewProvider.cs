using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Providers;

public interface IEstoquePedidoVendaEstoqueLocalViewProvider
{
    public Task<PagedResultDto<EstoquePedidoVendaEstoqueLocalViewOutput>> GetList(ListEstoquePedidoVendaEstoqueLocalInput input);
    public Task<EstoquePedidoVendaEstoqueLocalViewOutput> GetById(Guid id);
}