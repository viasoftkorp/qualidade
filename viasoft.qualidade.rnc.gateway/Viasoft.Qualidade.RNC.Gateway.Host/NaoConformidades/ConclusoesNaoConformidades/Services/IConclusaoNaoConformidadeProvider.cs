using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

public interface IConclusaoNaoConformidadeProvider
{
    public Task<HttpResponseMessage> ConcluirNaoConformidade(Guid idNaoConformidade, ConclusaoNaoConformidadeInput input);
    public Task<HttpResponseMessage> Estornar(Guid idNaoConformidade);
    public Task<HttpResponseMessage> CalcularCicloTempo(Guid idNaoConformidade);
    public Task<ConclusaoNaoConformidadeOutput> Get(Guid idNaoConformidade);
}