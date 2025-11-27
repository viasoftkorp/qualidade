using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public interface IProdutoNaoConformidadeProvider
{
    Task<ProdutoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade);

    Task<PagedResultDto<ProdutoNaoConformidadeViewOutput>> GetList(PagedFilteredAndSortedRequestInput input,
        Guid idNaoConformidade);

    Task<HttpResponseMessage> Create(ProdutoNaoConformidadeInput input, Guid idNaoConformidade);
    Task<HttpResponseMessage> Update(Guid id, ProdutoNaoConformidadeInput input, Guid idNaoConformidade);
    Task Delete(Guid id, Guid idNaoConformidade);
}