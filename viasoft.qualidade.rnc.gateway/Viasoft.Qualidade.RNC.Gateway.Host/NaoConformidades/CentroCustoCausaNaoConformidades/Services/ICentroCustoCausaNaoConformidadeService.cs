using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Services;

public interface ICentroCustoCausaNaoConformidadeService
{
    public Task<HttpResponseMessage> GetList(Guid idNaoConformidade, Guid idCausaNaoConformidade, PagedFilteredAndSortedRequestInput input);
    public Task<PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, PagedFilteredAndSortedRequestInput input);
}
