using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Services;

public interface INotaFiscalSaidaProvider
{
    public Task<PagedResultDto<NotaFiscalSaidaOutput>> GetList(GetListNotasFiscaisInput input);
    public Task<NotaFiscalSaidaOutput> GetById(Guid id);
}
public class GetListNotasFiscaisInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdCliente { get; set; }
}