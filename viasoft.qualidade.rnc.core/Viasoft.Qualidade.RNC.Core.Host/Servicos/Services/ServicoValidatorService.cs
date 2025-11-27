using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Servicos.Services;

public class ServicoValidatorService : IServicoValidatorService, ITransientDependency
{
    public bool ValidarTempo(int horas, int minutos)
    {
        if (horas < 0 || minutos < 0)
        {
            return false;
        }
        
        if (horas == 0 && minutos == 0)
        {
            return false;
        }

        return true;
    }
}