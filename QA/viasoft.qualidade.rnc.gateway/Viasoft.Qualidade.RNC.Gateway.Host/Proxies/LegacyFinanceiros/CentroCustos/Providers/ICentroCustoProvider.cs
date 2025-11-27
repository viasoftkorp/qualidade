using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;

public interface ICentroCustoProvider
{
    public Task<HttpResponseMessage> GetList(PagedFilteredAndSortedRequestInput input);
    public Task<HttpResponseMessage> GetById(Guid id);
}