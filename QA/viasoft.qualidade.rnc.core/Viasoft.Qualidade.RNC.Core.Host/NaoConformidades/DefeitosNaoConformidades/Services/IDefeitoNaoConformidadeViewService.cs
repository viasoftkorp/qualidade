using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public interface IDefeitoNaoConformidadeViewService
{
    Task<PagedResultDto<DefeitoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        PagedFilteredAndSortedRequestInput input);
}