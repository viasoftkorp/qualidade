using System.Collections.Generic;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class GetCategoriasListInput : PagedFilteredAndSortedRequestInput
{
    public GetCategoriasListInput(List<string> input)
    {
        CodigosCategorias = new List<string>();
        foreach (var codigo in input)
        {
            CodigosCategorias.Add(codigo);
        }
    }

    public List<string> CodigosCategorias { get; set; }
}