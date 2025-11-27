using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Services;

public interface INotaFiscalEntradaProvider
{
    public Task<PagedResultDto<NotaFiscalEntradaOutput>> GetList(GetListNotaFiscalInput input);
    public Task<NotaFiscalEntradaOutput> GetById(Guid id);
}
public class GetListNotaFiscalInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdFornecedor { get; set; }
    public Guid? IdEmpresa { get; set; }
}