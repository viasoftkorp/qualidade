using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Configuracoes.Gerais.Dtos;

public class ConfiguracaoGeralOutput : ConfiguracaoGeralModel
{
    public bool UtilizarReservaDePedidoNaLocalizacaoDeEstoque { get; set; }
    public string FrontendUrl { get; set; }
    public ConfiguracaoGeralOutput()
    {
        
    }

    public ConfiguracaoGeralOutput(ConfiguracaoGeral configuracao)
    {
        ConsiderarApenasSaldoApontado = configuracao.ConsiderarApenasSaldoApontado;
    }
}