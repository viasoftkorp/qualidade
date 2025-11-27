using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Services;

public interface INaoConformidadeProvider
{
    Task<NaoConformidadeOutput> Get(Guid idNaoConformidade);
    Task<PagedResultDto<NaoConformidadeViewOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<NaoConformidadeViewOutput> GetView(Guid idNaoConformidade);
    Task<HttpResponseMessage> Create(NaoConformidadeInput input);
    Task<HttpResponseMessage> Update(Guid id, NaoConformidadeInput input);
    Task<HttpResponseMessage> Delete(Guid id);
    Task<AgregacaoNaoConformidadeOutput> GetAgregacao(Guid id);
}