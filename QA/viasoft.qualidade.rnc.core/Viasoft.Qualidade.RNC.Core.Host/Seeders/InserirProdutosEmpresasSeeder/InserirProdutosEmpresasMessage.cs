using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.InserirProdutosEmpresasSeeder;

[Endpoint("Viasoft.Qualidade.RNC.Core.InserirProdutosEmpresasSeeder", "Viasoft.Qualidade.RNC.Core")]
public class InserirProdutosEmpresasSeederCommand : ICommand
{
}

[Endpoint("Viasoft.Qualidade.RNC.Core.InserirProdutosEmpresasSeeder")]
public class InserirProdutosEmpresasSeederMessage: IMessage
{
}