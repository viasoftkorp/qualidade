using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public interface IAcaoPreventivaNaoConformidadeViewService
{
    Task<PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, Guid idDefeitoNaoConformidade, 
        GetListWithDefeitoIdFlagInput input);
}