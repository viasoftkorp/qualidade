using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;

public interface IServicoNaoConformidadeViewService
{
    Task<PagedResultDto<ServicoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, 
        PagedFilteredAndSortedRequestInput input);
}