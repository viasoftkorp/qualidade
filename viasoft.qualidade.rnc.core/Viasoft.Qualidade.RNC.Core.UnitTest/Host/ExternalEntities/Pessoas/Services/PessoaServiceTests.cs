using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Pessoas.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Clientes;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.Pessoas.Services;

public class PessoaServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se pessoa já cadastrada, nada deve acontecer")]
    public async Task InserirProdutoSeNaoExistirTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.Clientes.InsertAsync(GetPessoaMock(0), true);

        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.PersonProxyService.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se pessoa não cadastrada, deve buscar pessoa e cadastra-la")]
    public async Task InserirProdutoSeNaoExistirTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var pessoaASerInserido = new PersonOutput()
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Code = TestUtils.ObjectMother.Ints[0].ToString(),
            CompanyName = TestUtils.ObjectMother.Strings[0]
        };

        mocker.PersonProxyService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(pessoaASerInserido);

        var expectedResult = new Cliente()
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        };
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var pessoaInserida = await mocker.Clientes.FindAsync(TestUtils.ObjectMother.Guids[0]);
        pessoaInserida.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    private Cliente GetPessoaMock(int index)
    {
        var pessoa = new Cliente
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index].ToString(),
            RazaoSocial = TestUtils.ObjectMother.Strings[index],
        };
        return pessoa;
    }

    private class Mocker
    {
        public IPersonProxyService PersonProxyService { get; set; }
        public IRepository<Cliente> Clientes { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
           PersonProxyService = Substitute.For<IPersonProxyService>(),
           Clientes = ServiceProvider.GetService<IRepository<Cliente>>()
        };

        return mocker;
    }

    private PessoaService GetService(Mocker mocker)
    {
        var service = new PessoaService(mocker.Clientes, mocker.PersonProxyService, UnitOfWork);

        return service;
    }
}