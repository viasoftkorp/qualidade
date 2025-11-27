using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Naturezas.Services;

public interface INaturezaService
{
    Task<NaturezaOutput> Get(Guid id);
    Task<PagedResultDto<NaturezaOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<PagedResultDto<NaturezaViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<ValidationResult> Create(NaturezaInput input);
    Task<ValidationResult> Update(Guid id, NaturezaInput input);
    Task<ValidationResult> Delete(Guid id);
    Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo);
}