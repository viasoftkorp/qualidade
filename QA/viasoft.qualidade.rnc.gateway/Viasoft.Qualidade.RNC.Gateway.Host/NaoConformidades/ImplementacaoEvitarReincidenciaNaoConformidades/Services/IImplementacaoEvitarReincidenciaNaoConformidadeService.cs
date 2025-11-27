using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

public interface IImplementacaoEvitarReincidenciaNaoConformidadeService
{
    public Task<PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, GetListViewInput input);

    public Task<HttpResponseMessageResult> GetById(Guid id, Guid idNaoConformidade);

    public Task<HttpResponseMessageResult> Insert(Guid idNaoConformidade, ImplementacaoEvitarReincidenciaNaoConformidadeInput input);

    public Task<HttpResponseMessageResult> Update(Guid idNaoConformidade, Guid id, ImplementacaoEvitarReincidenciaNaoConformidadeInput input);

    public Task<HttpResponseMessageResult> Remove(Guid idNaoConformidade, Guid id);
}