using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirUsuariosSolucoesSeeder;

[Endpoint("Viasoft.Qualidade.RNC.Core.CorrigirUsuariosSolucoes", "Viasoft.Qualidade.RNC.Core")]
public class CorrigirUsuariosSolucoesCommand : ICommand
{
}

[Endpoint("Viasoft.Qualidade.RNC.Core.CorrigirUsuariosSolucoes")]
public class CorrigirUsuariosSolucoesMessage: IMessage
{
}