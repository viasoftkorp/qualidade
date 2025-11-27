using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCodigoRecursosSeeders.Contracts;

[Endpoint("Viasoft.Qualidade.RNC.Core.SeedPreencherCodigoRecursos", "Viasoft.Qualidade.RNC.Core")]

public class SeedPreencherCodigoRecursosCommand : ICommand
{
    
}

[Endpoint("Viasoft.Qualidade.RNC.Core.SeedPreencherCodigoRecursos")]
public class SeedPreencherCodigoRecursosMessage : IMessage
{
    
}