using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Services;

public interface IAcaoPreventivaProvider
{
    Task<AcaoPreventivaOutput> Get(Guid id);
    Task<HttpResponseMessage> GetList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> Create(AcaoPreventivaInput input);
    Task<HttpResponseMessage> Update(Guid id, AcaoPreventivaInput input);
    Task<HttpResponseMessage> Delete(Guid id);
    Task<HttpResponseMessage> Inativar(Guid id);
    Task<HttpResponseMessage> Ativar(Guid id);
}