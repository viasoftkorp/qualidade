using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Services;

public interface IServicoNaoConformidadeProvider
{
    Task<ServicoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade);

    Task<PagedResultDto<ServicoNaoConformidadeViewOutput>> GetList(PagedFilteredAndSortedRequestInput input,
        Guid idNaoConformidade, Guid idSolucao);

    Task<HttpResponseMessage> Create(ServicoNaoConformidadeInput input, Guid idNaoConformidade);
    Task<HttpResponseMessage> Update(Guid id, ServicoNaoConformidadeInput input, Guid idNaoConformidade);
    Task Delete(Guid id, Guid idNaoConformidade);
}