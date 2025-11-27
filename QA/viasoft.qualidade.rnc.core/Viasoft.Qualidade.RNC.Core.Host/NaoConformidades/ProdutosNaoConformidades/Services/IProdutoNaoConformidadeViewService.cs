using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public interface IProdutoNaoConformidadeViewService
{
    Task<PagedResultDto<ProdutoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, 
        PagedFilteredAndSortedRequestInput input);
}