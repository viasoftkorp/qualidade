using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Providers;

public interface IPersonProvider
{
    Task<PersonOutput> GetById(Guid id);
    Task<PagedResultDto<PersonOutput>> GetAll(GetAllPersonInput input);
}