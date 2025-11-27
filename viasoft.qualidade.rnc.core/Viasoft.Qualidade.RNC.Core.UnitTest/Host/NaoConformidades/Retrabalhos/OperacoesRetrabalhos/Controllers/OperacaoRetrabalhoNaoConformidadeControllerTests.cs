using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Recursos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.
    Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.
    Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.
    Services;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Controllers;

public class OperacaoRetrabalhoNaoConformidadeControllerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se sucesso ao gerar operação, deve retornar OK")]
    public async Task CreateTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100,
        };
        mocker.OperacaoRetrabalhoNaoConformidadeService.Create(TestUtils.ObjectMother.Guids[0], input)
            .Returns(new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Success = true,
                ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok
            });

        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Success = true
        };
        // Act
        var result = await service.Create(TestUtils.ObjectMother.Guids[0], input);

        // Assert

        var actionResult = result.Result as OkResult;
        actionResult.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Se falhar ao gerar operação e retornar mensagem, deve retornar UnprocessableEntity")]
    public async Task CreateTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100
        };
        mocker.OperacaoRetrabalhoNaoConformidadeService.Create(TestUtils.ObjectMother.Guids[0], input)
            .Returns(new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Success = false,
                Message = "Erro de validação",
                ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.OdfFinalizada
            });

        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "Erro de validação",
            ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.OdfFinalizada
        };
        // Act
        var result = await service.Create(TestUtils.ObjectMother.Guids[0], input);

        // Assert

        var actionResult = result.Result as UnprocessableEntityObjectResult;
        actionResult.Value.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se houver maquinas, deve encontrar ids recursos e chamar BatchInserirRecursosNaoCadastrados")]
    public async Task CreateTest3()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var maquinas = new List<MaquinaInput>
        {
            GetMaquinaInputMock(0, contagemMateriais: 2),
            GetMaquinaInputMock(1, contagemMateriais: 2),
            GetMaquinaInputMock(2, contagemMateriais: 2),
            GetMaquinaInputMock(2, contagemMateriais: 1),
        };
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100,
            Maquinas = maquinas
        };
        mocker.OperacaoRetrabalhoNaoConformidadeService.Create(TestUtils.ObjectMother.Guids[0], input)
            .Returns(new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Success = true,
                ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok
            });

        var expectedRecursosIds = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
        };
        // Act
        await service.Create(TestUtils.ObjectMother.Guids[0], input);

        // Assert
        await mocker.RecursoService.Received(1).BatchInserirNaoCadastrados(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(expectedRecursosIds)));
    }
    [Fact(DisplayName = "Se houver materiais, deve encontrar ids produtos e chamar BatchInserirProdutosNaoCadastrados")]
    public async Task CreateTest4()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var maquinas = new List<MaquinaInput>
        {
            GetMaquinaInputMock(0, contagemMateriais: 5),
            GetMaquinaInputMock(1, contagemMateriais: 3),
            GetMaquinaInputMock(2, contagemMateriais: 4),
        };
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100,
            Maquinas = maquinas
        };
        mocker.OperacaoRetrabalhoNaoConformidadeService.Create(TestUtils.ObjectMother.Guids[0], input)
            .Returns(new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Success = true,
                ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok
            });

        var expectedProdutosIds = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };
        // Act
        await service.Create(TestUtils.ObjectMother.Guids[0], input);

        // Assert
        await mocker.ProdutoService.Received(1).BatchInserirNaoCadastrados(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(expectedProdutosIds)));
    }

    private MaquinaInput GetMaquinaInputMock(int index, int contagemMateriais)
    {
        var maquina = new MaquinaInput
        {
            IdRecurso = TestUtils.ObjectMother.Guids[index],
            Id = TestUtils.ObjectMother.Guids[index],
            Detalhamento = TestUtils.ObjectMother.Strings[index],
            Horas = TestUtils.ObjectMother.Ints[index],
            Minutos = TestUtils.ObjectMother.Ints[index]
        };
        for (int i = 0; i < contagemMateriais; i++)
        {
            var material = GetMaterialInputMock(i);
            maquina.Materiais.Add(material);
        }

        return maquina;
    }

    private MaterialInput GetMaterialInputMock(int index)
    {
        var material = new MaterialInput
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Detalhamento = TestUtils.ObjectMother.Strings[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdMaquina = TestUtils.ObjectMother.Guids[index],
            IdProduto = TestUtils.ObjectMother.Guids[index]
        };
        return material;
    }

    private class Mocker
    {
        public IOperacaoRetrabalhoNaoConformidadeService OperacaoRetrabalhoNaoConformidadeService { get; set; }
        public IProdutoService ProdutoService { get; set; }
        public IRecursoService RecursoService { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            OperacaoRetrabalhoNaoConformidadeService = Substitute.For<IOperacaoRetrabalhoNaoConformidadeService>(),
            RecursoService = Substitute.For<IRecursoService>(),
            ProdutoService = Substitute.For<IProdutoService>()
        };

        return mocker;
    }

    private OperacaoRetrabalhoNaoConformidadeController GetService(Mocker mocker)
    {
        var service = new OperacaoRetrabalhoNaoConformidadeController(mocker.OperacaoRetrabalhoNaoConformidadeService,
            mocker.ProdutoService, mocker.RecursoService);

        return service;
    }
}