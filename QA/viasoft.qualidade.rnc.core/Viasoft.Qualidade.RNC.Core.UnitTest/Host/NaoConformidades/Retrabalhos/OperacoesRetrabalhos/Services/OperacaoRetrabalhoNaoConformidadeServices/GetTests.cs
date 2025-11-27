using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeServices;

public class GetTests : OperacaoRetrabalhoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Se houver operação retrabalho não conformidade, deve retorna-la")]
    public async Task GetTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        agregacaoNaoConformidade.NaoConformidade.OperacaoRetrabalho = new OperacaoRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 1,
            IdNaoConformidade = agregacaoNaoConformidade.NaoConformidade.Id,
            NumeroOperacaoARetrabalhar = "010",
        };
        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);
        
        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            IdNaoConformidade = agregacaoNaoConformidade.NaoConformidade.Id,
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 1,
            Id = TestUtils.ObjectMother.Guids[0],
            Success = true,
            Operacoes = new List<OperacaoOutput>()
        };
        // Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se não houver operação retrabalho não conformidade, deve retornar null")]
    public async Task GetTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);
        
        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            IdNaoConformidade = agregacaoNaoConformidade.NaoConformidade.Id,
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 1,
            Id = TestUtils.ObjectMother.Guids[0],
            Success = true,
            Operacoes = new List<OperacaoOutput>()
        };
        // Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        output.Should().BeNull();
    }
}