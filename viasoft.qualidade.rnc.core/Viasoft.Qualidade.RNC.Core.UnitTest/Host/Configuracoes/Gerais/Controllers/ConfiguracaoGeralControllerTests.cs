using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Host.Configuracoes.Gerais.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.Configuracoes.Gerais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.FrontendUrls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Configuracoes.Gerais.Controllers;

public class ConfiguracaoGeralControllerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se método chamado, deve retornar configuracoes gerais")]
    public async Task GetTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        await mocker.Repository.InsertAsync(new ConfiguracaoGeral
        {
            ConsiderarApenasSaldoApontado = true
        }, true);
        mocker.FrontendUrl.Value.Returns(TestUtils.ObjectMother.Strings[0]);
        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(true);
        var expectedResult = new ConfiguracaoGeralOutput
        {
            ConsiderarApenasSaldoApontado = true,
            UtilizarReservaDePedidoNaLocalizacaoDeEstoque = true,
            FrontendUrl = TestUtils.ObjectMother.Strings[0]
        };
        //Act
        var result = await controller.Get();
        //Assert
        var actionResult = result.Result as OkObjectResult;
        actionResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    [Fact(DisplayName = "Se método chamado, deve atualizar configuracaoes com base no input")]
    public async Task UpdateTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        await mocker.Repository.InsertAsync(new ConfiguracaoGeral
        {
            ConsiderarApenasSaldoApontado = false
        }, true);
        var expectedResult = new ConfiguracaoGeral
        {
            ConsiderarApenasSaldoApontado = true
        };
        var input = new ConfiguracaoGeralInput
        {
            ConsiderarApenasSaldoApontado = true
        };
        //Act
        var result = await controller.Update(input);
        //Assert
        var configuracaoResult = await mocker.Repository.FirstAsync();
        configuracaoResult.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options).Excluding(e => e.Id));
    }
    
    protected class Mocker
    {
        public IRepository<ConfiguracaoGeral> Repository { get; set; }
        public ILegacyParametrosProvider LegacyParametrosProvider{ get; set; }
        public IFrontendUrl FrontendUrl{ get; set; }
        
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            Repository = ServiceProvider.GetService<IRepository<ConfiguracaoGeral>>(),
            LegacyParametrosProvider = Substitute.For<ILegacyParametrosProvider>(),
            FrontendUrl = Substitute.For<IFrontendUrl>()
        };
        return mocker;
    }

    protected ConfiguracaoGeralController GetController(Mocker mocker)
    {
        var controller = new ConfiguracaoGeralController(mocker.Repository, mocker.LegacyParametrosProvider,
            mocker.FrontendUrl);
        return controller;
    }
}