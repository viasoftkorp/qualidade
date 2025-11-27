using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

public interface IReclamacaoNaoConformidadeProvider
{
    Task<HttpResponseMessage> Insert(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input);
    Task<HttpResponseMessage> Update(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input);
    Task<ReclamacaoNaoConformidadeOutput> Get(Guid idNaoConformidade);
}