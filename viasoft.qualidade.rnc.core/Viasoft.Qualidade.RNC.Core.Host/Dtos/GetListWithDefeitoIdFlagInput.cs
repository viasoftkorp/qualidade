using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Core.Host.Dtos;

public class GetListWithDefeitoIdFlagInput : PagedFilteredAndSortedRequestInput
{
    public bool UsarIdDefeito { get; set; }
}