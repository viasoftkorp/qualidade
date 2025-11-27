using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Dtos
{
    public class GetAllPersonInput : PagedFilteredAndSortedRequestInput
    {
        public PersonType TipoPessoa { get; set; }
    }
}