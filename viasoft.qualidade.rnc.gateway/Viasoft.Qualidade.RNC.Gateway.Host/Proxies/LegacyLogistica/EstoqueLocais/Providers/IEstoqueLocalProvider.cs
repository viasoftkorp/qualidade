using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Providers;

public interface IEstoqueLocalProvider
{
    public Task<PagedResultDto<EstoqueLocalOutput>> GetList(GetListEstoqueLocalInput input);
    public Task<EstoqueLocalOutput> GetById(Guid id);
}

public class GetListEstoqueLocalInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
    public Guid? IdProduto { get; set; }
    public string NumeroLote { get; set; }
    public Guid? IdPedido { get; set; }
}