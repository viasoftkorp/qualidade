using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Providers;

public interface ILocalProvider
{
    public Task<PagedResultDto<LocalOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    public Task<LocalOutput> GetById(Guid id);
}