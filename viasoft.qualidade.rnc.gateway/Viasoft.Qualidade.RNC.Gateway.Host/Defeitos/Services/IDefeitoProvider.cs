using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Services;

public interface IDefeitoProvider
{
    Task<DefeitoOutput> Get(Guid id);
    Task<PagedResultDto<DefeitoOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> Create(DefeitoInput input);
    Task<HttpResponseMessage> Update(Guid id, DefeitoInput input);
    Task<HttpResponseMessage> Delete(Guid id);
    Task<HttpResponseMessage> Inativar(Guid id);
    Task<HttpResponseMessage> Ativar(Guid id);
}