using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Services;

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