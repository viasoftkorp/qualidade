using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public class ProdutoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get ProdutoNaoConformidade with Success")]
    public async Task GetProdutoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetProdutoNaoConformidade(0);

        await mocker.ProdutoNaoConformidadeRepository.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Get(input.IdNaoConformidade, input.Id);

        //Assert
        var produtoSolucao = await mocker.ProdutoNaoConformidadeRepository.FindAsync(TestUtils.ObjectMother.Guids[0]);
        produtoSolucao.Should().BeEquivalentTo(output, options => options.Excluding(e => e.CompanyId));
    }


    [Fact(DisplayName = "Get ProdutoNaoConformidade Returns Null")]
    public async Task GetProdutoNaoConformidadeWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetProdutoNaoConformidade(0);

        await mocker.ProdutoNaoConformidadeRepository.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Get(input.IdNaoConformidade, TestUtils.ObjectMother.Guids[3]);

        //Assert
        output.Should().BeNull();
    }

    [Fact(DisplayName = "Insert ProdutoNaoConformidade with Success")]
    public async Task InsertProdutoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var produtoNaoConformidadeInput = new ProdutoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0]
        };
        var expectedResult = new ProdutoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Insert(idNaoConformidade, produtoNaoConformidadeInput);
        //Assert
        var result = naoConformidade.ProdutoNaoConformidades
            .Find(p => p.Id.Equals(produtoNaoConformidadeInput.Id));
        result.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Update ProdutoNaoConformidade with Success")]
    public async Task UpdateProdutoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var produtoSolucaoInput = new ProdutoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[1],
            IdProduto = TestUtils.ObjectMother.Guids[0]
        };
        var expectedResult = new ProdutoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[1],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0]
        };

        await service.Insert(idNaoConformidade, produtoSolucaoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Update(idNaoConformidade, produtoSolucaoInput.Id, produtoSolucaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(produtoSolucaoInput.IdNaoConformidade);
        var produtoNaoConformidade = agregacao.ProdutoNaoConformidades.Find(p => p.IdNaoConformidade.Equals(idNaoConformidade));
        produtoNaoConformidade.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Remove ProdutoNaoConformidade with Success")]
    public async Task RemoveProdutoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var produtoNaoConformidadeInput = new ProdutoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[2],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, produtoNaoConformidadeInput);
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Remove(idNaoConformidade, produtoNaoConformidadeInput.Id);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        var produtoSolucao = agregacao.ProdutoNaoConformidades.Find(p => p.Id.Equals(produtoNaoConformidadeInput.Id));
        produtoSolucao.Should().BeNull();
    }

    private ProdutoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new ProdutoNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment = TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            ProdutoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<ProdutoNaoConformidade>>(),
            CurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];

        return mocker;
    }

    private ProdutoNaoConformidadeService GetService(ProdutoNaoConformidadeServiceMocker mocker)
    {
        var service = new ProdutoNaoConformidadeService(mocker.NaoConformidadeRepository,
            mocker.DateTimeProvider, mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, 
            mocker.ProdutoNaoConformidadeRepository, mocker.CurrentCompany);

        return service;
    }

    public class ProdutoNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<ProdutoNaoConformidade> ProdutoNaoConformidadeRepository { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }

    }
}