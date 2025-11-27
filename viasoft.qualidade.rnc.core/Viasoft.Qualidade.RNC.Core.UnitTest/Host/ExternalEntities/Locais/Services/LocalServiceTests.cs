using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.Locais.Services;

public class LocalServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se local já cadastrado, nada deve acontecer")]
    public async Task InserirLocalSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.Locais.InsertAsync(GetLocalMock(0), true);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.LocalProvider.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se local não cadastrado, deve buscar local e cadastra-lo")]
    public async Task InserirLocalSeNaoExistirTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var localASerInserido = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };

        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localASerInserido);

        var expectedResult = new Local
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var localInserido = await mocker.Locais.FindAsync(TestUtils.ObjectMother.Guids[0]);
        localInserido.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se houver local, para cada local não cadastrado, deve cadastra-lo")]
    public async Task BatchInserirLocalSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await CadastrarLocais(mocker, 3);

        var idsLocais = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };
        var locals = new List<LocalOutput>
        {
            new LocalOutput(GetLocalMock(3)),
            new LocalOutput(GetLocalMock(4)),
        };
        MockarRetornoGetAllLocaisByListaIdsPaginando(mocker, idsLocais, locals);

        var expectedResult = new List<Local>
        {
            GetLocalMock(0),
            GetLocalMock(1),
            GetLocalMock(2),
            GetLocalMock(3),
            GetLocalMock(4),
        };

        var idsLocaisParaInserir = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };

        //Act
        await service.BatchInserirNaoCadastrados(idsLocaisParaInserir);
        //Assert
        var localsResult = await mocker.Locais.ToListAsync();
        localsResult.Should().BeEquivalentTo(expectedResult);
    }

    private void MockarRetornoGetAllLocaisByListaIdsPaginando(Mocker mocker, List<Guid> expectedIds,
        List<LocalOutput> expectedOutput)
    {
        mocker.LocalProvider.GetAllByIdsPaginando(Arg.Is<List<Guid>>(e =>
            e.SequenceEqual(expectedIds))).Returns(expectedOutput);
    }

    private async Task CadastrarLocais(Mocker mocker, int numeroLocaisAInserir)
    {
        for (int i = 0; i < numeroLocaisAInserir; i++)
        {
            var local = GetLocalMock(i);
            await mocker.Locais.InsertAsync(local);
        }

        await UnitOfWork.SaveChangesAsync();
    }

    private Local GetLocalMock(int index)
    {
        var local = new Local
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index],
            Descricao = TestUtils.ObjectMother.Strings[index],
            IsBloquearMovimentacao = true
        };
        return local;
    }

    private class Mocker
    {
        public IRepository<Local> Locais { get; set; }
        public ILocalProvider LocalProvider { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            Locais = ServiceProvider.GetService<IRepository<Local>>(),
            LocalProvider = Substitute.For<ILocalProvider>()
        };

        return mocker;
    }

    private LocalService GetService(Mocker mocker)
    {
        var service = new LocalService(mocker.Locais, mocker.LocalProvider, UnitOfWork);

        return service;
    }
}