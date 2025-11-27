using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Dtos;

public class GetListWithDefeitoIdFlagInput : PagedFilteredAndSortedRequestInput
{
    public bool? UsarIdDefeito { get; set; }
}