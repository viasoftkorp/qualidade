using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public interface IDefeitoNaoConformidadeProvider
{
    Task<DefeitoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade);

    Task<PagedResultDto<DefeitoNaoConformidadeViewOutput>> GetList(PagedFilteredAndSortedRequestInput input,
        Guid idNaoConformidade);

    Task<HttpResponseMessage> Create(DefeitoNaoConformidadeInput input, Guid idNaoConformidade);
    Task<HttpResponseMessage> Update(Guid id, DefeitoNaoConformidadeInput input, Guid idNaoConformidade);
    Task Delete(Guid id, Guid idNaoConformidade);
}