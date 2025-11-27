using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Naturezas.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Naturezas.Services;

public interface INaturezaProvider
{
    Task<NaturezaOutput> Get(Guid id);
    Task<PagedResultDto<NaturezaOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> Create(NaturezaInput input);
    Task<HttpResponseMessage> Update(Guid id, NaturezaInput input);
    Task<HttpResponseMessage> Delete(Guid id);
    Task<HttpResponseMessage> Inativar(Guid id);
    Task<HttpResponseMessage> Ativar(Guid id);
}