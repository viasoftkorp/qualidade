using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Services;

public interface IAcaoPreventivaService
{
    Task<AcaoPreventivaOutput> Get(Guid id);
    Task<PagedResultDto<AcaoPreventivaOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<PagedResultDto<AcaoPreventivaViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<ValidationResult> Create(AcaoPreventivaInput input);
    Task<ValidationResult> Update(Guid id, AcaoPreventivaInput input);
    Task<ValidationResult> Delete(Guid id);
    Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo);
    

}