using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public interface ISolucaoNaoConformidadeViewService
{
    Task<PagedResultDto<SolucaoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, Guid idDefeito,
        GetListWithDefeitoIdFlagInput input);
}