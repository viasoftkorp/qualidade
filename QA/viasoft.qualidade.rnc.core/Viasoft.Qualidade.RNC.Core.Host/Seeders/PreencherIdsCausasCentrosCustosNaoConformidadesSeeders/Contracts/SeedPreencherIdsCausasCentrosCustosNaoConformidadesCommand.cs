using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdsCausasCentrosCustosNaoConformidadesSeeders.Contracts;

[Endpoint("Viasoft.Qualidade.RNC.Core.SeedPreencherIdsCausasCentrosCustosNaoConformidades", "Viasoft.Qualidade.RNC.Core")]
public class SeedPreencherIdsCausasCentrosCustosNaoConformidadesCommand : ICommand
{
}

[Endpoint("Viasoft.Qualidade.RNC.Core.SeedPreencherIdsCausasCentrosCustosNaoConformidades")]
public class SeedPreencherIdsCausasCentrosCustosNaoConformidadesMessage : IMessage
{
}
