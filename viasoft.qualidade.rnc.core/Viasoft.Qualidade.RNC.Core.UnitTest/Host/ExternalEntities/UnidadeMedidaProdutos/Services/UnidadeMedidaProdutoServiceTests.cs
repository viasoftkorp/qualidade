using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.UnidadeMedidaProdutos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.UnidadeMedidaProdutos.Services;

public class UnidadeMedidaProdutoServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se unidade medida já cadastrada, nada deve acontecer")]
    public async Task InserirUnidadeMedidaSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.UnidadeMedidaProdutos.InsertAsync(GetUnidadeMedidaProdutoMock(0), true);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.UnidadeMedidaProdutoProxyService.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se unidade medida não cadastrada, deve buscar unidade medida e cadastra-la")]
    public async Task InserirUnidadeMedidaSeNaoExistirTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var unidadeMedidaProdutoASerInserida = new UnidadeMedidaProdutoOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            UnidadeMedida = TestUtils.ObjectMother.Strings[0]
        };

        mocker.UnidadeMedidaProdutoProxyService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(unidadeMedidaProdutoASerInserida);

        var expectedResult = GetUnidadeMedidaProdutoMock(0);
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var unidadeMedidaProdutoInserido = await mocker.UnidadeMedidaProdutos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        unidadeMedidaProdutoInserido.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }
    
    [Fact(DisplayName = "Se houver unidade medida, para cada unidade medida não cadastrada, deve cadastra-la")]
    public async Task BatchInserirUnidadeMedidaSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        await CadastrarUnidadeMedidaProdutos(mocker, 3);
        
        var indicesUnidadeMedidaProdutosNaoCadastradas = new List<int>
        {
            3, 4
        };
        
        MockarRetornoGetAllUnidadeMedidaProdutosByListaIdsPaginando(mocker, indicesUnidadeMedidaProdutosNaoCadastradas);
        
        var expectedResult = new List<UnidadeMedidaProduto>
        {
            GetUnidadeMedidaProdutoMock(0),
            GetUnidadeMedidaProdutoMock(1),
            GetUnidadeMedidaProdutoMock(2),
            GetUnidadeMedidaProdutoMock(3),
            GetUnidadeMedidaProdutoMock(4),
        };
        var idsUnidadesParaInserir = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };
        
        //Act
        await service.BatchInserirNaoCadastrados(idsUnidadesParaInserir);
        //Assert
        var produtosResult = await mocker.UnidadeMedidaProdutos.ToListAsync();
        produtosResult.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }
    
    private UnidadeMedidaProduto GetUnidadeMedidaProdutoMock(int index)
    {
        var unidadeMedidaProduto = new UnidadeMedidaProduto
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Descricao = TestUtils.ObjectMother.Strings[index],
        };
        return unidadeMedidaProduto;
    }
    
    private async Task CadastrarUnidadeMedidaProdutos(Mocker mocker, int numeroProdutosAInserir)
    {
        
        for (int i = 0; i < numeroProdutosAInserir; i++)
        {
            var unidadeMedidaProduto = GetUnidadeMedidaProdutoMock(i);
            
            await mocker.UnidadeMedidaProdutos.InsertAsync(unidadeMedidaProduto);
        }

        await UnitOfWork.SaveChangesAsync();
    }
    
    private void MockarRetornoGetAllUnidadeMedidaProdutosByListaIdsPaginando(Mocker mocker, List<int> indexes)
    {
        var unidadeMedidaToReturn = new List<UnidadeMedidaProdutoOutput>();
        var idsToMock = new List<Guid>();

        foreach (var index in indexes)
        {
            idsToMock.Add(TestUtils.ObjectMother.Guids[index]);

            var unidadeToMock = GetUnidadeMedidaProdutoMock(index);

            var unidadeMedidaProdutoToReturn = new UnidadeMedidaProdutoOutput
            {
                Id = unidadeToMock.Id,
                UnidadeMedida = unidadeToMock.Descricao,
            };

            unidadeMedidaToReturn.Add(unidadeMedidaProdutoToReturn);
        }

        mocker.UnidadeMedidaProdutoProxyService.GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(idsToMock))).Returns(unidadeMedidaToReturn);
    }

    private class Mocker
    {
        public IRepository<UnidadeMedidaProduto> UnidadeMedidaProdutos { get; set; }
        public IUnidadeMedidaProdutoProxyService UnidadeMedidaProdutoProxyService { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            UnidadeMedidaProdutos = ServiceProvider.GetService<IRepository<UnidadeMedidaProduto>>(),
            UnidadeMedidaProdutoProxyService = Substitute.For<IUnidadeMedidaProdutoProxyService>()
        };

        return mocker;
    }

    private UnidadeMedidaProdutoService GetService(Mocker mocker)
    {
        var service = new UnidadeMedidaProdutoService(mocker.UnidadeMedidaProdutos,
            mocker.UnidadeMedidaProdutoProxyService, UnitOfWork);

        return service;
    }
}