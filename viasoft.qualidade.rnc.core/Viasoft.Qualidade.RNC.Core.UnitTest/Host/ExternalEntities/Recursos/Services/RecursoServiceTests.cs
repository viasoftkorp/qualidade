using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Recursos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Recursos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.Recursos.Services;

public class RecursoServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se recurso já cadastrado, nada deve acontecer")]
    public async Task InserirRecursoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.Recursos.InsertAsync(GetRecursoMock(0), true);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.RecursosProxyService.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se recurso não cadastrado, deve buscar recurso e cadastra-lo")]
    public async Task InserirRecursoSeNaoExistirTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var recursoASerInserido = new RecursoOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
        };

        mocker.RecursosProxyService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(recursoASerInserido);

        var expectedResult = new Recurso
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
        };
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var recursoInserido = await mocker.Recursos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        recursoInserido.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Se houver recurso, para cada recurso não cadastrado, deve cadastra-lo")]
    public async Task BatchInserirRecursoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await CadastrarRecursos(mocker, 3);

        var indicesRecursosNaoCadastrados = new List<int>
        {
            3, 4
        };
        MockarRetornoGetAllRecursosPaginando(mocker, indicesRecursosNaoCadastrados);

        var expectedResult = new List<Recurso>
        {
            GetRecursoMock(0),
            GetRecursoMock(1),
            GetRecursoMock(2),
            GetRecursoMock(3),
            GetRecursoMock(4),
        };

        var idsRecursosParaInserir = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };

        //Act
        await service.BatchInserirNaoCadastrados(idsRecursosParaInserir);
        //Assert
        var recursosResult = await mocker.Recursos.ToListAsync();
        recursosResult.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    private void MockarRetornoGetAllRecursosPaginando(Mocker mocker, List<int> indexes)
    {
        var recursosToReturn = new List<RecursoOutput>();
        var idsToMock = new List<Guid>();

        foreach (var index in indexes)
        {
            idsToMock.Add(TestUtils.ObjectMother.Guids[index]);

            var recursoMock = GetRecursoMock(index);

            var recursoToReturn = new RecursoOutput
            {
                Id = recursoMock.Id,
                Descricao = recursoMock.Descricao,
                Codigo = recursoMock.Codigo
            };

            recursosToReturn.Add(recursoToReturn);
        }

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(idsToMock)))
            .Returns(recursosToReturn);
    }

    private async Task CadastrarRecursos(Mocker mocker, int numeroRecursosAInserir)
    {
        for (int i = 0; i < numeroRecursosAInserir; i++)
        {
            var recurso = GetRecursoMock(i);
            await mocker.Recursos.InsertAsync(recurso);
        }

        await UnitOfWork.SaveChangesAsync();
    }

    private Recurso GetRecursoMock(int index)
    {
        var recurso = new Recurso
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[index],
        };
        return recurso;
    }

    private class Mocker
    {
        public IRepository<Recurso> Recursos {get;set;}
        public IRecursosProxyService RecursosProxyService {get;set;}
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            Recursos = ServiceProvider.GetService<IRepository<Recurso>>(),
            RecursosProxyService = Substitute.For<IRecursosProxyService>()
        };

        return mocker;
    }

    private RecursoService GetService(Mocker mocker)
    {
        var service = new RecursoService(mocker.Recursos, mocker.RecursosProxyService, UnitOfWork);

        return service;
    }
}