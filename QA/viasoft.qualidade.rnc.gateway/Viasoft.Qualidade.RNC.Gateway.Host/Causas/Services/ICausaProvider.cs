using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;


namespace Viasoft.Qualidade.RNC.Gateway.Host.Causas.Services;

public interface ICausaProvider
{
    Task<CausaOutput> Get(Guid id);
    Task<PagedResultDto<CausaOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> Create(CausaInput input);
    Task<HttpResponseMessage> Update(Guid id, CausaInput input);
    Task<HttpResponseMessage> Delete(Guid id);
    Task<HttpResponseMessage> Inativar(Guid id);
    Task<HttpResponseMessage> Ativar(Guid id);
}