using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Causas.Services;

public interface ICausaService
{
    Task<CausaOutput> Get(Guid id);
    Task<PagedResultDto<CausaOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<PagedResultDto<CausaViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<ValidationResult> Create(CausaInput input);
    Task<ValidationResult> Update(Guid id, CausaInput input);
    Task<ValidationResult> Delete(Guid id);
    Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo);
}