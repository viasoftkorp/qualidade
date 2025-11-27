using System.Collections.Generic;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Core.Domain.Utils;

public class PagelessResultDto<T> : ListResultDto<T>
{
    public PagelessResultDto()
    {
    }
    public PagelessResultDto(List<T> items) : base(items)
    {
    }
}