using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;

public interface INaoConformidadeViewService
{
    Task<PagedResultDto<NaoConformidadeViewOutput>> GetListView(PagedFilteredAndSortedRequestInput input);
    Task<NaoConformidadeViewOutput> GetView(Guid idNaoConformidade);
}