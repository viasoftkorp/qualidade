using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendaEstoqueLocais.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendaEstoqueLocais.Providers;

public interface IEstoquePedidoVendaEstoqueLocalProvider
{
    public Task<PagedResultDto<EstoquePedidoVendaEstoqueLocalOutput>> GetList(GetListEstoqueLocalInput input);
    public Task<EstoquePedidoVendaEstoqueLocalOutput> GetById(Guid id);
}