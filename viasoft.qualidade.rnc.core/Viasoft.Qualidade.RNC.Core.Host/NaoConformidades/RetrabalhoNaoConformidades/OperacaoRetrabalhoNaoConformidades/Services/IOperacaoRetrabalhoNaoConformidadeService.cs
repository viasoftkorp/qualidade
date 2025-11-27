using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Services;

public interface IOperacaoRetrabalhoNaoConformidadeService
{
    public Task<OperacaoRetrabalhoNaoConformidadeOutput> Create(Guid idNaoConformidade, OperacaoRetrabalhoNaoConformidadeInput input);
    public Task<OperacaoRetrabalhoNaoConformidadeOutput> Get(Guid idNaoConformidade);

    public Task<PagedResultDto<OperacaoViewOutput>> GetOperacoesView(Guid idOperacaoRetrabalho,
        PagedFilteredAndSortedRequestInput input);
}