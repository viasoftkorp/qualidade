using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.CentroCustos;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.CentroCustos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.CentroCustos.Services;

public class CentroCustoServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se centroCusto já cadastrado, nada deve acontecer")]
    public async Task InserirCentroCustoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.CentroCustos.InsertAsync(GetCentroCustoMock(0), true);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.CentroCustoProvider.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se centroCusto não cadastrado, deve buscar centroCusto e cadastra-lo")]
    public async Task InserirCentroCustoSeNaoExistirTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var centroCustoASerInserido = new CentroCustoOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsSintetico = false
        };

        mocker.CentroCustoProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(centroCustoASerInserido);

        var expectedResult = new CentroCusto
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
        };
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var centroCustoInserido = await mocker.CentroCustos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        centroCustoInserido.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se houver centro custo, para cada centro custo não cadastrado, deve cadastra-lo")]
    public async Task BatchInserirCentroCustoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await CadastrarCentroCustos(mocker, 3);

        var idsCentroCustos = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };
        var centroCustos = new List<CentroCustoOutput>
        {
            new CentroCustoOutput(GetCentroCustoMock(3)),
            new CentroCustoOutput(GetCentroCustoMock(4)),
        };
        MockarRetornoGetAllCentroCustosByListaIdsPaginando(mocker, idsCentroCustos, centroCustos);

        var expectedResult = new List<CentroCusto>
        {
            GetCentroCustoMock(0),
            GetCentroCustoMock(1),
            GetCentroCustoMock(2),
            GetCentroCustoMock(3),
            GetCentroCustoMock(4),
        };

        var idsCentroCustosParaInserir = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };

        //Act
        await service.BatchInserirNaoCadastrados(idsCentroCustosParaInserir);
        //Assert
        var centroCustosResult = await mocker.CentroCustos.ToListAsync();
        centroCustosResult.Should().BeEquivalentTo(expectedResult);
    }

    private void MockarRetornoGetAllCentroCustosByListaIdsPaginando(Mocker mocker, List<Guid> expectedIds, List<CentroCustoOutput> expectedOutput)
    {
        mocker.CentroCustoProvider.GetAllByIdsPaginando(Arg.Is<List<Guid>>(e =>
            e.SequenceEqual(expectedIds))).Returns(expectedOutput);
    }

    private async Task CadastrarCentroCustos(Mocker mocker, int numeroCentroCustosAInserir)
    {
        for (int i = 0; i < numeroCentroCustosAInserir; i++)
        {
            var centroCusto = GetCentroCustoMock(i);
            await mocker.CentroCustos.InsertAsync(centroCusto);
        }

        await UnitOfWork.SaveChangesAsync();
    }

    private CentroCusto GetCentroCustoMock(int index)
    {
        var centroCusto = new CentroCusto
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[index],
        };
        return centroCusto;
    }

    private class Mocker
    {
        public IRepository<CentroCusto> CentroCustos {get;set;}
        public ICentroCustoProvider CentroCustoProvider {get;set;}
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            CentroCustos = ServiceProvider.GetService<IRepository<CentroCusto>>(),
            CentroCustoProvider = Substitute.For<ICentroCustoProvider>()
        };

        return mocker;
    }

    private CentroCustoService GetService(Mocker mocker)
    {
        var service = new CentroCustoService(mocker.CentroCustos, mocker.CentroCustoProvider, UnitOfWork);

        return service;
    }
}