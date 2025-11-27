using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public interface IAcaoPreventivaNaoConformidadeProvider
{
    Task<AcaoPreventivaNaoConformidadeViewOutput> Get(Guid idNaoConformidade, Guid id);

    Task<PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>> GetList(Guid idNaoConformidade,
        Guid idDefeitoNaoConformidade,
        GetListWithDefeitoIdFlagInput input, bool usarIdDefeito);

    Task<HttpResponseMessage> Create(Guid idNaoConformidade, AcaoPreventivaNaoConformidadeInput input);
    Task<HttpResponseMessage> Update(Guid idNaoConformidade, Guid id, AcaoPreventivaNaoConformidadeInput input);
    Task Delete(Guid idNaoConformidade, Guid id);
}