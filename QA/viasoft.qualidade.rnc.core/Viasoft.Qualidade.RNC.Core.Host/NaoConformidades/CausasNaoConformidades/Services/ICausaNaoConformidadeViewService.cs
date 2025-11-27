using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;

public interface ICausaNaoConformidadeViewService
{

    Task<PagedResultDto<CausaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,Guid idDefeitoNaoConformidade,
        GetListWithDefeitoIdFlagInput input);
}