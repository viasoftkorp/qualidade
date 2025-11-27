using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Services;

public interface ICausaNaoConformidadeProvider
{
    Task<CausaNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade);

    Task<PagedResultDto<CausaNaoConformidadeViewOutput>> GetList(GetListWithDefeitoIdFlagInput input,
        Guid idNaoConformidade, Guid idDefeito, bool usarIdDefeito);

    Task<HttpResponseMessage> Create([FromBody] CausaNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade);

    Task<HttpResponseMessage> Update([FromRoute] Guid id, [FromBody] CausaNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade);

    Task Delete([FromRoute] Guid id, [FromRoute] Guid idNaoConformidade);
}