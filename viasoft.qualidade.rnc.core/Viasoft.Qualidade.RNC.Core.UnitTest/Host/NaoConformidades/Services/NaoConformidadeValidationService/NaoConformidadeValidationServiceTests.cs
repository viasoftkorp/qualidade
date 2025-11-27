using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public abstract class NaoConformidadeValidationServiceTest
{
    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            OrdemProducaoProvider = Substitute.For<IOrdemProducaoProvider>(),
            ItemNotaFiscalEntradaProvider = Substitute.For<IItemNotaFiscalEntradaProvider>(),
            ItemNotaFiscalEntradaRateioLoteProvider = Substitute.For<IItemNotaFiscalEntradaRateioLoteProvider>()
        };
        return mocker;
    }

    protected Core.Host.NaoConformidades.Services.NaoConformidadeValidationService GetService(Mocker mocker)
    {

        var service = new Core.Host.NaoConformidades.Services.NaoConformidadeValidationService(mocker.ItemNotaFiscalEntradaProvider,
            mocker.ItemNotaFiscalEntradaRateioLoteProvider, mocker.OrdemProducaoProvider);

        return service;
    }

    protected class Mocker
    {
        public IItemNotaFiscalEntradaProvider ItemNotaFiscalEntradaProvider {get;set;}
        public IItemNotaFiscalEntradaRateioLoteProvider ItemNotaFiscalEntradaRateioLoteProvider {get;set;}
        public IOrdemProducaoProvider OrdemProducaoProvider {get; set; }
    }
}