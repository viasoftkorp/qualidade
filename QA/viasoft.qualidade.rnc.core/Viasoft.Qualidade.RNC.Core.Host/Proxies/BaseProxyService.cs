using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies;

public abstract class BaseProxyService<TEntityOutput> : IBaseProxyService<TEntityOutput>
{
    public PagedFilteredAndSortedRequestInput GetAllFilter = new PagedFilteredAndSortedRequestInput
    {
        SkipCount = 0,
        MaxResultCount = MaxResultCount,
    };
    
    public abstract Task<ListResultDto<TEntityOutput>> GetAll(PagedFilteredAndSortedRequestInput filter);
    public abstract Task<TEntityOutput> GetById(Guid id);

    protected abstract JsonNetFilterRule GetGetAllAdvancedFilter(object value);
    
    private const int MaxResultCount = 50;
    
    public async Task<List<TEntityOutput>> GetAllByIdsPaginando(List<Guid> ids)
    {
        if (ids.Count == 0)
        {
            return new List<TEntityOutput>();
        }
        
        var idsDistintos = ids.Distinct().ToList();
        var skipCount = 0;

        var result = new List<TEntityOutput>();

        var numeroBuscas = Math.Ceiling((double)idsDistintos.Count / MaxResultCount);
        for (int i = 0; i < numeroBuscas; i++)
        {
            var idsPaginados = idsDistintos.Skip(skipCount).Take(MaxResultCount).ToList();

            var advancedFilter = GetGetAllAdvancedFilter(idsPaginados);
            
            GetAllFilter.AdvancedFilter = JsonConvert.SerializeObject(advancedFilter);
            
            var entidadesPaginadas = await GetAll(GetAllFilter);
            
            result.AddRange(entidadesPaginadas.Items);
            
            skipCount += MaxResultCount;
        }

        return result;  
    }
}