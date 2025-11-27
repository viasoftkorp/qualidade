using Viasoft.Qualidade.RNC.Core.Host.Servicos.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Servicos.Services.ServicoValidatorServices;

public abstract class ServicoValidatorServiceTest: TestUtils.UnitTestBaseWithDbContext
{
    protected SolucaoServiceMocker GetMocker()
    {
        var mocker = new SolucaoServiceMocker
        {
        };
        return mocker;
    }

    protected ServicoValidatorService GetService(SolucaoServiceMocker mocker)
    {
        var service = new ServicoValidatorService();
        return service;
    }

    protected class SolucaoServiceMocker
    {
    }
}