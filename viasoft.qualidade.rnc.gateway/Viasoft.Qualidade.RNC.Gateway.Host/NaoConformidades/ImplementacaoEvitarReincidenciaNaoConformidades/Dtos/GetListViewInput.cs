using System;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

public class GetListViewInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdDefeito { get; set; }
}