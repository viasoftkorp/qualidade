using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Services.Recursos;

public interface IRecursoProvider
{
    Task<PagedResultDto<RecursoOutput>> GetRecursosList(PagedFilteredAndSortedRequestInput input);

    Task<RecursoOutput> GetRecurso(Guid id);
}