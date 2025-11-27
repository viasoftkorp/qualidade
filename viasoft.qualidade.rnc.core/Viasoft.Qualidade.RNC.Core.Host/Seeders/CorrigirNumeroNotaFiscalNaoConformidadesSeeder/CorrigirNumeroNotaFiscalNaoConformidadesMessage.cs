using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNumeroNotaFiscalNaoConformidadesSeeder;

[Endpoint("Viasoft.Qualidade.RNC.Core.CorrigirNumeroNotaFiscalNaoConformidades", "Viasoft.Qualidade.RNC.Core")]
public class CorrigirNumeroNotaFiscalNaoConformidadesCommand : ICommand
{
}

[Endpoint("Viasoft.Qualidade.RNC.Core.CorrigirNumeroNotaFiscalNaoConformidades")]
public class CorrigirNumeroNotaFiscalNaoConformidadesMessage: IMessage
{
}