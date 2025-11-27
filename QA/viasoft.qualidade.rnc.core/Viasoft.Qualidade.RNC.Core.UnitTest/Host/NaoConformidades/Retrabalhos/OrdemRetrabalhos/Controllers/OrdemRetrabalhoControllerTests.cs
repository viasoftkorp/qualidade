using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Rebus.TestHelpers.Events;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Commands;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Controllers;

public class OrdemRetrabalhoControllerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se não pode gerar ordem de retrabalho, deve retornar unprocessableEntity com o motivo")]
    public async Task GerarOrdemRetrabalhoTest1()
    {
        //Arrange 
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        mocker.NaoConformidadeRepository.Get(TestUtils.ObjectMother.Guids[0]).Returns(agregacao);

        mocker.GerarOrdemRetrabalhoValidatorService
            .ValidateStatusRnc()
            .ValidateOperacaoEngenhariaFinal()
            .ValidateOperacaoEngenhariaDuplicada()
            .ValidateOdf()
            .ValidateLote(input)
            .ValidateQuantidade(input)
            .ValidateAsync(agregacao)
            .Returns(GerarOrdemRetrabalhoValidationResult.LoteObrigatorio);

        //Act
        var result = await controller.GerarOrdemRetrabalho(TestUtils.ObjectMother.Guids[0], input);

        //Assert
        var actionResult = result.Result as UnprocessableEntityObjectResult;
        var value = actionResult.Value as OrdemRetrabalhoNaoConformidadeOutput;
        value.Success.Should().BeFalse();
        value.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "Se sucesso ao gerar odf de retrabalho, deve mandar movimentar estoque")]
    public async Task GerarOrdemRetrabalhoTest2()
    {
        //Arrange 
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var expectedResult = new MovimentarEstoqueOrdemRetrabalhoCommand
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        mocker.OrdemRetrabalhoService.CanGenerate(TestUtils.ObjectMother.Guids[0], input, true)
            .Returns(GerarOrdemRetrabalhoValidationResult.Ok);

        mocker.NaoConformidadeRepository.Get(TestUtils.ObjectMother.Guids[0]).Returns(agregacao);
        mocker.OrdemRetrabalhoService.GerarOrdemRetrabalho(agregacao, input).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            NumeroOdfRetrabalho = 1
        });
        
        //Act
        var result = await controller.GerarOrdemRetrabalho(TestUtils.ObjectMother.Guids[0], input);

        //Assert
        var message = ServiceBus.FakeBus.Events.OfType<MessageSentToSelf<MovimentarEstoqueOrdemRetrabalhoCommand>>()
            .First().CommandMessage;
        message.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName =
        "Se não der sucesso ao gerar odf de retrabalho, deve retornar unprocessableEntity com o motivo")]
    public async Task GerarOrdemRetrabalhoTest3()
    {
        //Arrange 
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        mocker.OrdemRetrabalhoService.CanGenerate(TestUtils.ObjectMother.Guids[0], input, true)
            .Returns(GerarOrdemRetrabalhoValidationResult.Ok);

        mocker.NaoConformidadeRepository.Get(TestUtils.ObjectMother.Guids[0]).Returns(agregacao);
        mocker.OrdemRetrabalhoService.GerarOrdemRetrabalho(agregacao, input).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "Erro"
        });

        //Act
        var result = await controller.GerarOrdemRetrabalho(TestUtils.ObjectMother.Guids[0], input);

        //Assert
        ServiceBus.FakeBus.Events.OfType<MessageSentToSelf<MovimentarEstoqueOrdemRetrabalhoCommand>>().Should()
            .BeEmpty();
        var actionResult = result.Result as UnprocessableEntityObjectResult;
        var value = actionResult.Value as OrdemRetrabalhoNaoConformidadeOutput;
        value.Success.Should().BeFalse();
        value.Message.Should().Be("Erro");
    }

    [Fact(DisplayName = "Se não pode estornar ordem de retrabalho, deve retornar unprocessableEntity com o motivo")]
    public async Task EstornarOrdemRetrabalhoTest1()
    {
        //Arrange 
        var mocker = GetMocker();
        var controller = GetController(mocker);
        mocker.OrdemRetrabalhoService.CanEstornar(TestUtils.ObjectMother.Guids[0])
            .Returns(EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaApontada);

        //Act
        var result = await controller.EstornarOrdemRetrabalho(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var actionResult = result.Result as UnprocessableEntityObjectResult;
        var value = actionResult.Value as OrdemRetrabalhoNaoConformidadeOutput;
        value.Success.Should().BeFalse();
        value.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "Se sucesso ao estornar odf de retrabalho, deve mandar movimentar estoque")]
    public async Task EstornarOrdemRetrabalhoTest2()
    {
        //Arrange 
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var expectedResult = new MovimentarEstoqueOrdemRetrabalhoCommand
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = true
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Quantidade = 1,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);
        mocker.OrdemRetrabalhoService.CanEstornar(TestUtils.ObjectMother.Guids[0])
            .Returns(EstornarOrdemRetrabalhoValidationResult.Ok);

        MockGetAgregacaoReturn(mocker, agregacao);

        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, ordemRetrabalhoNaoConformidade, true)
            .Returns(new OrdemRetrabalhoNaoConformidadeOutput());
        
        //Act
        var result = await controller.EstornarOrdemRetrabalho(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var message = ServiceBus.FakeBus.Events.OfType<MessageSentToSelf<MovimentarEstoqueOrdemRetrabalhoCommand>>()
            .First().CommandMessage;
        message.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se falha ao estornar odf de retrabalho, deve retornar unprocessableEntity com o motivo")]
    public async Task EstornarOrdemRetrabalhoTest3()
    {
        //Arrange 
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Quantidade = 1,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);
        mocker.OrdemRetrabalhoService.CanEstornar(TestUtils.ObjectMother.Guids[0])
            .Returns(EstornarOrdemRetrabalhoValidationResult.Ok);

        MockGetAgregacaoReturn(mocker, agregacao);

        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, ordemRetrabalhoNaoConformidade, true).Returns(
            new OrdemRetrabalhoNaoConformidadeOutput
            {
                Success = false,
                Message = "Erro"
            });

        //Act
        var result = await controller.EstornarOrdemRetrabalho(TestUtils.ObjectMother.Guids[0]);

        //Assert
        ServiceBus.FakeBus.Events.OfType<MessageSentToSelf<MovimentarEstoqueOrdemRetrabalhoCommand>>().Should()
            .BeEmpty();
        var actionResult = result.Result as UnprocessableEntityObjectResult;
        var value = actionResult.Value as OrdemRetrabalhoNaoConformidadeOutput;
        value.Success.Should().BeFalse();
        value.Message.Should().Be("Erro");
    }

    private class Mocker
    {
        public IOrdemRetrabalhoService OrdemRetrabalhoService { get; set; }
        public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidades { get; set; }
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public IGerarOrdemRetrabalhoValidatorService GerarOrdemRetrabalhoValidatorService { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            OrdemRetrabalhoService = Substitute.For<IOrdemRetrabalhoService>(),
            OrdemRetrabalhoNaoConformidades = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>(),
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            GerarOrdemRetrabalhoValidatorService = Substitute.For<IGerarOrdemRetrabalhoValidatorService>()
        };
        return mocker;
    }

    private OrdemRetrabalhoController GetController(Mocker mocker)
    {
        var controller = new OrdemRetrabalhoController(mocker.OrdemRetrabalhoService,
            mocker.OrdemRetrabalhoNaoConformidades,mocker.NaoConformidadeRepository, ServiceBus,
            mocker.GerarOrdemRetrabalhoValidatorService);
        return controller;
    }

    private void MockGetAgregacaoReturn(Mocker mocker, AgregacaoNaoConformidade agregacao)
    {
        mocker.NaoConformidadeRepository.Get(agregacao.NaoConformidade.Id)
            .Returns(agregacao);
    }
}