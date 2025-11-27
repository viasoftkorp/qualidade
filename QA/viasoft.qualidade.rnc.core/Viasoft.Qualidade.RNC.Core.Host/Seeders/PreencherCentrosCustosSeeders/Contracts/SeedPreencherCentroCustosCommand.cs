using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCentrosCustosSeeders.Contracts;

[Endpoint("Viasoft.Qualidade.RNC.Core.SeedPreencherCentroCustos", "Viasoft.Qualidade.RNC.Core")]

public class SeedPreencherCentroCustosCommand : ICommand
{
    
}

[Endpoint("Viasoft.Qualidade.RNC.Core.SeedPreencherCentroCustos")]
public class SeedPreencherCentroCustosMessage : IMessage
{
    
}