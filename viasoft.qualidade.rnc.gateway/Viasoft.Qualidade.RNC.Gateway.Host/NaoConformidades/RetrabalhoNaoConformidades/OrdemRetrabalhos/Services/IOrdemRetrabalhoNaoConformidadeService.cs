using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;

public interface IOrdemRetrabalhoNaoConformidadeService
{
    public Task<HttpResponseMessage> GerarOrdemRetrabalho(Guid idNaoConformidade, OrdemRetrabalhoInput input);
    public Task<HttpResponseMessage> GetCanGenerateOrdemRetrabalho(Guid idNaoConformidade, bool isFullValidation);
    public Task<HttpResponseMessage> EstornarOrdemRetrabalho(Guid idNaoConformidade);
    public Task<HttpResponseMessage> Get(Guid idNaoConformidade);
    public Task<OrdemRetrabalhoNaoConformidadeViewOutput> GetView(Guid idNaoConformidade);
}