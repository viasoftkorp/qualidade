using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.UnidadeMedidaProdutos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.Produtos.Services;

public class ProdutoServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se produto já cadastrado, nada deve acontecer")]
    public async Task InserirProdutoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.Produtos.InsertAsync(GetProdutoMock(0), true);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.ProdutoProxyService.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se produto não cadastrado, deve buscar produto e cadastra-lo")]
    public async Task InserirProdutoSeNaoExistirTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produtoASerInserido = new ProdutoOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
            IdUnidade = TestUtils.ObjectMother.Guids[0]
        };

        mocker.ProdutoProxyService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(produtoASerInserido);

        var expectedResult = new Produto
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var produtoInserido = await mocker.Produtos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        produtoInserido.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Se produto cadastrado, deve chamar InserirUnidadeMedidaSeNaoExistir")]
    public async Task InserirProdutoSeNaoExistirTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produtoASerInserido = new ProdutoOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[0],
            IdUnidade = TestUtils.ObjectMother.Guids[0]
        };

        mocker.ProdutoProxyService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(produtoASerInserido);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.UnidadeMedidaProdutoService.Received(1)
            .InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se houver produto, para cada produto não cadastrado, deve cadastra-lo")]
    public async Task BatchInserirProdutoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await CadastrarProdutos(mocker, 3);

        var indicesProdutosNaoCadastrados = new List<int>
        {
            3, 4
        };
        MockarRetornoGetAllProdutosByListaIdsPaginando(mocker, indicesProdutosNaoCadastrados);

        var expectedResult = new List<Produto>
        {
            GetProdutoMock(0),
            GetProdutoMock(1),
            GetProdutoMock(2),
            GetProdutoMock(3),
            GetProdutoMock(4),
        };

        var idsProdutosParaInserir = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
        };

        //Act
        await service.BatchInserirNaoCadastrados(idsProdutosParaInserir);
        //Assert
        var produtosResult = await mocker.Produtos.ToListAsync();
        produtosResult.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    private void MockarRetornoGetAllProdutosByListaIdsPaginando(Mocker mocker, List<int> indexes)
    {
        var produtosToReturn = new List<ProdutoOutput>();
        var idsToMock = new List<Guid>();

        foreach (var index in indexes)
        {
            idsToMock.Add(TestUtils.ObjectMother.Guids[index]);

            var produtoMock = GetProdutoMock(index);

            var produtoToReturn = new ProdutoOutput
            {
                Id = produtoMock.Id,
                Descricao = produtoMock.Descricao,
                IdCategoria = produtoMock.IdCategoria,
                IdUnidade = produtoMock.IdUnidadeMedida,
                Codigo = produtoMock.Codigo
            };

            produtosToReturn.Add(produtoToReturn);
        }

        mocker.ProdutoProxyService.GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(idsToMock))).Returns(produtosToReturn);
    }

    private async Task CadastrarProdutos(Mocker mocker, int numeroProdutosAInserir)
    {
        for (int i = 0; i < numeroProdutosAInserir; i++)
        {
            var produto = GetProdutoMock(i);
            await mocker.Produtos.InsertAsync(produto);
        }

        await UnitOfWork.SaveChangesAsync();
    }

    private Produto GetProdutoMock(int index)
    {
        var produto = new Produto
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[index],
            IdCategoria = TestUtils.ObjectMother.Guids[index],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[index]
        };
        return produto;
    }

    private class Mocker
    {
        public IProdutosProxyService ProdutoProxyService { get; set; }
        public IRepository<Produto> Produtos { get; set; }
        public IUnidadeMedidaProdutoService UnidadeMedidaProdutoService { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            ProdutoProxyService = Substitute.For<IProdutosProxyService>(),
            Produtos = ServiceProvider.GetService<IRepository<Produto>>(),
            UnidadeMedidaProdutoService = Substitute.For<IUnidadeMedidaProdutoService>()
        };

        return mocker;
    }

    private ProdutoService GetService(Mocker mocker)
    {
        var service = new ProdutoService(mocker.ProdutoProxyService, mocker.Produtos,
            mocker.UnidadeMedidaProdutoService, UnitOfWork);

        return service;
    }
}