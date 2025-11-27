using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Defeitos.Services;

public interface IDefeitoService
{
    Task<DefeitoOutput> Get(Guid id);
    Task<PagedResultDto<DefeitoOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<PagedResultDto<DefeitoViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<ValidationResult> Create(DefeitoInput input);
    Task<ValidationResult> Update(Guid id, DefeitoInput input);
    Task<ValidationResult> Delete(Guid id);
    Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo);

}