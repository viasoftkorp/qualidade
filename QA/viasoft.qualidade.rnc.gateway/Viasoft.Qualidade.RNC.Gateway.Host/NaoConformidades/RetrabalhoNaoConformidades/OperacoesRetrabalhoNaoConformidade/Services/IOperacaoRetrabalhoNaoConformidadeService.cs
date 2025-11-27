using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Services;

public interface IOperacaoRetrabalhoNaoConformidadeService
{
    public Task<HttpResponseMessage> Create(Guid idNaoConformidade, OperacaoRetrabalhoNaoConformidadeInput input);
    public Task<OperacaoRetrabalhoNaoConformidade> Get(Guid idNaoConformidade);

    public Task<PagedResultDto<OperacaoViewOutput>> GetOperacoesView(Guid idNaoConformidade, Guid idOperacaoRetrabalho, 
        PagedFilteredAndSortedRequestInput input);
}