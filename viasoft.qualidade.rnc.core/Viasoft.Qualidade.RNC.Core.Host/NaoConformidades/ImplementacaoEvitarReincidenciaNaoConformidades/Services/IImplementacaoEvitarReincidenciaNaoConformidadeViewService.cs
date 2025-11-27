using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

public interface IImplementacaoEvitarReincidenciaNaoConformidadeViewService
{
    public Task<PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        GetListViewInput input);
    
    
}

public class GetListViewInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdDefeito { get; set; }
}